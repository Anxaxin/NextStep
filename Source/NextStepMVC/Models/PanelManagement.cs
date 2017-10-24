namespace NextStepMVC.Models
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;
  using System.Text.RegularExpressions;
  using System.Web;
  using System.Web.Helpers;

  public class PanelManagement
  {
    private readonly RetrieveContent rc;
    private CMSDbEntities ef;

    public PanelManagement()
    {
      this.rc = new RetrieveContent();
      this.rc.GetIndexing();
    }

    public void AddPanel(int styleSheet, string menuTag, string underMenuTag)
    {
      using (this.ef = new CMSDbEntities())
      {
        var style = this.ef.PanelStyles.Find(styleSheet);

          var query = from b in this.ef.Panels
              where b.MenuTag == menuTag
              where b.PageSettingId == this.rc.Style.ThisPageId
              select b;

                var query2 = from b in this.ef.Panels
                     orderby b.MenuIndex
                     where b.PageSettingId == this.rc.Style.ThisPageId
                     select b;

                if (query.Any())
        {
          var highest = 1;
          foreach (var item in query)
          {
            if (item.UnderMenuIndex > highest)
            {
              highest = Convert.ToInt32(item.UnderMenuIndex, CultureInfo.CurrentCulture);
            }
          }

          this.ef.Panels.Add(new Panel { MenuTag = menuTag, PanelStyle = style, UnderMenuTag = underMenuTag, PageSettingId = this.rc.Style.ThisPageId, MenuIndex = query.First().MenuIndex, UnderMenuIndex = highest + 1 });
          this.ef.SaveChanges();
        }
        else
        {
          var index = 1;
          foreach (var item in query2)
          {
            if (item.MenuIndex > index)
            {
              index = Convert.ToInt32(item.MenuIndex, CultureInfo.CurrentCulture);
            }
          }

          this.ef.Panels.Add(new Panel { MenuTag = menuTag, PanelStyle = style, UnderMenuTag = underMenuTag, PageSettingId = this.rc.Style.ThisPageId, MenuIndex = index + 1, UnderMenuIndex = 1 });
          this.ef.SaveChanges();
        }
      }
    }

    public void AddTextPanel(int id, string context, string header)
    {
      using (this.ef = new CMSDbEntities())
      {
        this.ef.TextPanels.Add(new TextPanel { Context = context, Header = header, P_Id = id });
        this.ef.SaveChanges();
      }
    }

    public void AddVideoPanel(int id, string video, string header)
    {
      using (this.ef = new CMSDbEntities())
      {
        const string Pattern = @"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/)([\w\-]+))(?:[^\s?]+)?)";
        const string Replacement = "$1";

        var rgx = new Regex(Pattern);
        var result = rgx.Replace(video, Replacement);

        this.ef.VideoTables.Add(new VideoTable { VideoHeader = header, VideoUrl = result, P_Id = id, VideoSize = "Large" });
        this.ef.SaveChanges();
      }
    }

    public void AddPicturePanel(int id, string header, HttpPostedFileBase thisFile)
    {
      if (thisFile == null)
      {
        return;
      }

      var pp = new PicturePanel();
      pp.Header = header;
      pp.P_Id = id;

      var lenght = thisFile.ContentLength;
      var tempimage = new byte[lenght];
      thisFile.InputStream.Read(tempimage, 0, lenght);
      pp.ImageData = tempimage;
      pp.ImageType = thisFile.ContentType;

      using (this.ef = new CMSDbEntities())
      {
        this.ef.PicturePanels.Add(pp);
        this.ef.SaveChanges();
      }
    }

    public void AddComboPanel(int id, string header, string context, string align, HttpPostedFileBase thisFile)
    {
        if (thisFile == null)
        {
        return;
        }

        var cp = new CombomPanel();
        cp.Context = context;
        cp.Header = header;
        cp.PictureAlign = align;
        cp.P_Id = id;

        var lenght = thisFile.ContentLength;
        var tempimage = new byte[lenght];
            
        thisFile.InputStream.Read(tempimage, 0, lenght);

        var wi = new WebImage(tempimage);
        var width = wi.Width;
        if (wi.Width > 279)
        {
            var ratio = 279 / width;
            if (ratio != 0)
            {
                wi.Resize(279, Convert.ToInt32(wi.Height * ratio));
                cp.ImageData = wi.GetBytes();
                cp.ImageType = thisFile.ContentType;
            }
        }
        else
        {
            cp.ImageData = wi.GetBytes();
            cp.ImageType = thisFile.ContentType;
        }

        using (this.ef = new CMSDbEntities())
        {
            this.ef.CombomPanels.Add(cp);
            this.ef.SaveChanges();
        }
    }

    public void AddGalleryPanel(int id, string header)
    {
      using (this.ef = new CMSDbEntities())
      {
        var gallery = new GalleryPanel()
        {
          Header = header,
          P_Id = id
        };
        this.ef.GalleryPanels.Add(gallery);
        this.ef.SaveChanges();
      }
    }

    public void AddGalleryImage(int id, HttpPostedFileBase thisFile)
    {
      if (thisFile == null)
      {
        return;
      }

      using (this.ef = new CMSDbEntities())
      {
        var image = new GalleryImage();
        image.Gallery_Id = id;

        var lenght = thisFile.ContentLength;
        var tempimage = new byte[lenght];
        thisFile.InputStream.Read(tempimage, 0, lenght);

        var wi = new WebImage(tempimage);
        var height = wi.Height;
        if (wi.Height > 150)
        {
          var ratio = 150 / height;
          if (ratio != 0)
          {
            wi.Resize(Convert.ToInt32(wi.Width * ratio), 150);
            image.imageThumb = wi.GetBytes();
            image.imageType = thisFile.ContentType;
            image.imageTitle = thisFile.FileName;
          }
        }
        else
        {
          image.imageThumb = tempimage;
          image.imageType = thisFile.ContentType;
        }

        var ifs = new ImageFullSize();
        ifs.imagedata = tempimage;
        ifs.imagetype = thisFile.ContentType;
        image.ImageFullSize = ifs;

        this.ef.GalleryImages.Add(image);
        this.ef.SaveChanges();
      }
    }

    public void EditComboPanel(int id, string menuTag, string underMenuTag, int style, HttpPostedFileBase thisFile, string align, string header, string context)
    {
      using (this.ef = new CMSDbEntities())
      {
        var combo = this.ef.Panels.Find(id);
        if (combo == null || combo.CombomPanel == null)
        {
          return;
        }

        combo.MenuTag = menuTag;
        combo.UnderMenuTag = underMenuTag;
        combo.StyleSheet = style;
        combo.CombomPanel.PictureAlign = align;
        combo.CombomPanel.Header = header;
        combo.CombomPanel.Context = context;

        if (thisFile != null && thisFile.ContentLength > 50)
        {
            var lenght = thisFile.ContentLength;
            var tempimage = new byte[lenght];

            thisFile.InputStream.Read(tempimage, 0, lenght);

            var wi = new WebImage(tempimage);
            var width = wi.Width;
            if (wi.Width > 279)
            {
                var ratio = 279 / width;
                if (ratio != 0)
                {
                    wi.Resize(279, Convert.ToInt32(wi.Height * ratio));
                    combo.CombomPanel.ImageData = wi.GetBytes();
                    combo.CombomPanel.ImageType = thisFile.ContentType;
                }
            }
            else
            {
                combo.CombomPanel.ImageData = wi.GetBytes();
                combo.CombomPanel.ImageType = thisFile.ContentType;
            }
        }

        this.ef.SaveChanges();
      }
    }

    public void EditTextPanel(int id, string menuTag, string underMenuTag, int style, string header, string context)
    {
      using (this.ef = new CMSDbEntities())
      {
        var text = this.ef.Panels.Find(id);
        if (text.TextPanel == null)
        {
          return;
        }

        text.MenuTag = menuTag;
        text.UnderMenuTag = underMenuTag;
        text.StyleSheet = style;
        text.TextPanel.Context = context;
        text.TextPanel.Header = header;
        this.ef.SaveChanges();
      }
    }

    public void EditPicturePanel(int id, string menuTag, string underMenuTag, int style, HttpPostedFileBase thisFile, string header)
    {
      if (thisFile == null)
      {
        return;
      }

      using (this.ef = new CMSDbEntities())
      {
        var pic = this.ef.Panels.Find(id);
        if (pic.PicturePanel == null)
        {
          return;
        }

        pic.MenuTag = menuTag;
        pic.UnderMenuTag = underMenuTag;
        pic.StyleSheet = style;
        pic.PicturePanel.Header = header;

        if (HttpPostedFileBaseExtensions.IsImage(thisFile))
        {
          var lenght = thisFile.ContentLength;
          var tempimage = new byte[lenght];
          thisFile.InputStream.Read(tempimage, 0, lenght);
          pic.PicturePanel.ImageData = tempimage;
          pic.PicturePanel.ImageType = thisFile.ContentType;
        }

        this.ef.SaveChanges();
      }
    }

    public void EditVideoPanel(int id, string menuTag, string underMenuTag, int style, string video, string videoHeader)
    {
      using (this.ef = new CMSDbEntities())
      {
        var vid = this.ef.Panels.Find(id);
        if (vid.VideoTable == null)
        {
          return;
        }

        vid.MenuTag = menuTag;
        vid.UnderMenuTag = underMenuTag;
        vid.StyleSheet = style;
        vid.VideoTable.VideoHeader = videoHeader;
        const string Pattern = @"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/)([\w\-]+))(?:[^\s?]+)?)";
        const string Replacement = "$1";
        var rgx = new Regex(Pattern);
        var result = rgx.Replace(video, Replacement);
        vid.VideoTable.VideoUrl = result;

        this.ef.SaveChanges();
      }
    }

    public void EditGalleryPanel(int id, int style, string header, string menuTag, string underMenuTag)
    {
      using (this.ef = new CMSDbEntities())
      {
        var gallery = this.ef.Panels.Find(id);
        if (gallery.GalleryPanel == null)
        {
          return;
        }

        gallery.MenuTag = menuTag;
        gallery.UnderMenuTag = underMenuTag;
        gallery.StyleSheet = style;
        gallery.GalleryPanel.Header = header;
        this.ef.SaveChanges();
      }
    }

    public int EditGalleryImage(int id, string description, string title)
    {
      using (this.ef = new CMSDbEntities())
      {
        var image = this.ef.GalleryImages.Find(id);
        var panelid = 0;
        var panelIndex = 0;
        if (image != null)
        {
          panelid = image.Gallery_Id;
          image.imageDescription = description;
          image.imageTitle = title;
          this.ef.SaveChanges();
        }

        foreach (var item in this.rc.Pan)
        {
          if (item.Panel_Id == panelid)
          {
            return panelIndex;
          }

          panelIndex++;
        }

        return panelid;
      }
    }

    public int DeleteGalleryImage(int id)
    {
      using (this.ef = new CMSDbEntities())
      {
        var image = this.ef.GalleryImages.Find(id);
        var panelid = 0;
        var panelIndex = 0;
        if (image != null)
        {
          panelid = image.Gallery_Id;
          this.ef.ImageFullSizes.Remove(image.ImageFullSize);
          this.ef.GalleryImages.Remove(image);
          this.ef.SaveChanges();
        }

        foreach (var item in this.rc.Pan)
        {
          if (item.Panel_Id == panelid)
          {
            return panelIndex;
          }

          panelIndex++;
        }

        return panelid;
      }
    }

    public void DeletePanel(int id)
    {
      using (this.ef = new CMSDbEntities())
      {
        var query = from b in this.ef.Panels.Include("PicturePanel").Include("VideoTable").Include("CombomPanel").Include("TextPanel").Include("PanelStyle").Include("GalleryPanel").Include("GalleryPanel.GalleryImages")
                    where b.Panel_Id == id
                    orderby b.Panel_Id
                    select b;

                if (query.Count() == 1)
        {
          if (query.First().GalleryPanel == null)
          {
            this.ef.Panels.Remove(query.First());
            this.ef.SaveChanges();
          }
          else
          {
            if (query.First().GalleryPanel.GalleryImages != null || query.First().GalleryPanel.GalleryImages.Count > 0)
            {
              foreach (var item in query.First().GalleryPanel.GalleryImages)
              {
                if (item.ImageFullSize != null)
                {
                  this.ef.ImageFullSizes.Remove(item.ImageFullSize);
                }
              }

              this.ef.GalleryImages.RemoveRange(query.First().GalleryPanel.GalleryImages);
            }

            this.ef.Panels.Remove(query.First());
            this.ef.SaveChanges();
          }
        }
      }
    }

    public void MovePanelIndexUp(int index)
    {
      if (index == 1)
      {
        return;
      }

      var these = new List<Panel>();
      var those = new List<Panel>();
      var thisIndex = 0;
      var takeThese = false;
      using (this.ef = new CMSDbEntities())
      {
        var query = from b in this.ef.Panels.OrderByDescending(e => e.MenuIndex)
                    where b.PageSettingId == this.rc.Style.ThisPageId
                    select b;

                var query2 = from b in this.ef.Panels
                     where b.MenuIndex == index
                     where b.PageSettingId == this.rc.Style.ThisPageId
                     orderby b.MenuIndex
                     select b;

                foreach (var qy in query2)
        {
          these.Add(qy);
        }

        foreach (var efitem in query)
        {
          if (efitem.MenuIndex >= index)
          {
          }
          else
          {
            if (thisIndex == 0)
            {
              thisIndex = Convert.ToInt32(efitem.MenuIndex, CultureInfo.CurrentCulture);
              takeThese = true;
            }
          }

          if (takeThese && thisIndex == efitem.MenuIndex)
          {
            those.Add(efitem);
          }
        }

        if (those.Any())
        {
          foreach (var item2 in these)
          {
            item2.MenuIndex = thisIndex;
          }

          foreach (var item3 in those)
          {
            item3.MenuIndex = index;
          }
        }

        this.ef.SaveChanges();
      }
    }

    public void MovePanelIndexDown(int index)
    {
      var these = new List<Panel>();
      var those = new List<Panel>();
      var thisIndex = 0;
      var takeThese = false;
      using (this.ef = new CMSDbEntities())
      {
        var query = from b in this.ef.Panels
                    where b.PageSettingId == this.rc.Style.ThisPageId
                    orderby b.MenuIndex
                    select b;

                var query2 = from b in this.ef.Panels
                     where b.MenuIndex == index
                     where b.PageSettingId == this.rc.Style.ThisPageId
                     orderby b.MenuIndex
                     select b;

                foreach (var qy in query2)
        {
          these.Add(qy);
        }

        foreach (var efitem in query)
        {
          if (efitem.MenuIndex <= index)
          {
          }
          else
          {
            if (thisIndex == 0)
            {
              thisIndex = Convert.ToInt32(efitem.MenuIndex, CultureInfo.CurrentCulture);
              takeThese = true;
            }
          }

          if (takeThese && thisIndex == efitem.MenuIndex)
          {
            those.Add(efitem);
          }
        }

        if (those.Any())
        {
          foreach (var item2 in these)
          {
            item2.MenuIndex = thisIndex;
          }

          foreach (var item3 in those)
          {
            item3.MenuIndex = index;
          }
        }

        this.ef.SaveChanges();
      }
    }

    public void MoveUnderMenuIndexUp(int index, int underIndex)
    {
      if (underIndex <= 1)
      {
        return;
      }

      using (this.ef = new CMSDbEntities())
      {
        var query2 = from b in this.ef.Panels
                     where b.MenuIndex == index
                     where b.PageSettingId == this.rc.Style.ThisPageId
                     orderby b.UnderMenuIndex
                     select b;

                if (query2.Count() <= 1)
        {
          return;
        }

        var thisnew = query2.First();
        var original = query2.First();

        foreach (var qy in query2)
        {
          if (underIndex > qy.UnderMenuIndex)
          {
            thisnew = qy;
          }
          else if (underIndex == qy.UnderMenuIndex)
          {
            original = qy;
          }
        }

        if (thisnew == original)
        {
          return;
        }

        var undertag = Convert.ToInt32(thisnew.UnderMenuIndex, CultureInfo.CurrentCulture);
        thisnew.UnderMenuIndex = original.UnderMenuIndex;
        original.UnderMenuIndex = undertag;
        this.ef.SaveChanges();
      }
    }

    public void MoveUnderMenuIndexDown(int index, int underIndex)
    {
      using (this.ef = new CMSDbEntities())
      {
        var query2 = from b in this.ef.Panels
                     where b.MenuIndex == index
                     where b.PageSettingId == this.rc.Style.ThisPageId
                     orderby b.UnderMenuIndex
                     select b;

                if (query2.Count() <= 1)
        {
          return;
        }

        var thisnew = query2.First();
        var original = query2.First();
        var occupied = false;

        foreach (var qy in query2)
        {
          if (underIndex < qy.UnderMenuIndex && occupied == false)
          {
            thisnew = qy;
            occupied = true;
          }
          else if (underIndex == qy.UnderMenuIndex)
          {
            original = qy;
          }
        }

        if (thisnew == original && thisnew == query2.First())
        {
          return;
        }

        var undertag = Convert.ToInt32(thisnew.UnderMenuIndex, CultureInfo.CurrentCulture);
        thisnew.UnderMenuIndex = original.UnderMenuIndex;
        original.UnderMenuIndex = undertag;
        this.ef.SaveChanges();
      }
    }
  }
}