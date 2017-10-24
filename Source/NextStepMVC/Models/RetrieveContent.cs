namespace NextStepMVC.Models
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Web;

    public class RetrieveContent
    {
        private CMSDbEntities ef;

        public RetrieveContent()
        {
            this.Style = new SiteStyle();
            this.RetrievePanelStyles();
        }

        public Collection<PanelStyle> PanelStyles { get; private set; }

        public SiteStyle Style { get; set; }

        public Collection<Panel> Pan { get; private set; }

        public Collection<AdminLogin> AdminLogins { get; set; }

        public void RetrievePanelStyles()
        {
            using (this.ef = new CMSDbEntities())
            {
                if (this.PanelStyles == null)
                {
                    Collection<PanelStyle> ps =
                        HttpContext.Current.Cache[this.Style.ThisPageId + "style"] as Collection<PanelStyle>;
                    if (ps == null)
                    {
                        this.PanelStyles = new Collection<PanelStyle>();
                        here:
                        var query = from b in this.ef.PanelStyles
                                    where b.PageSettingId == this.Style.ThisPageId
                                    orderby b.Id
                                    select b;

                        if (query.Any())
                        {
                            foreach (var item in query)
                            {
                                this.PanelStyles.Add(item);
                            }

                            HttpContext.Current.Cache[this.Style.ThisPageId + "style"] = this.PanelStyles;
                        }
                        else
                        {
                            this.ef.PanelStyles.Add(new PanelStyle { StyleName = "Default", color_panel_context = "003366", color_panel_title = "003366", color_panel_background = "ddddbb", font_family_panel_context = "Arial", font_family_panel_title = "Arial", PageSettingId = this.Style.ThisPageId });
                            this.ef.SaveChanges();
                            goto here;
                        }
                    }
                    else
                    {
                        this.PanelStyles = ps;
                    }
                }
            }
        }

        public void ListAdminUsers(int pageId)
        {
            if (this.AdminLogins == null)
            {
                this.AdminLogins = new Collection<AdminLogin>();
                using (this.ef = new CMSDbEntities())
                {
                    var query = from b in this.ef.AdminLogins
                                where b.PageSettingId == pageId
                                orderby b.user
                                select b;
                    foreach (var item in query)
                    {
                        this.AdminLogins.Add(item);
                    }
                }
            }
        }

        public void ClearPanelCache()
        {
            HttpContext.Current.Cache.Remove(this.Style.ThisPageId + "content");
        }

        public void ClearStyleCache()
        {
            HttpContext.Current.Cache.Remove(this.Style.ThisPageId + "style");
        }

        public void GetContent()
        {
            using (this.ef = new CMSDbEntities())
            {
                if (this.Pan == null)
                {
                    this.Pan = new Collection<Panel>();
                    Collection<Panel> panels = HttpContext.Current.Cache[this.Style.ThisPageId + "content"] as Collection<Panel>;
                    if (panels == null)
                    {
                        var query = from b in this.ef.Panels.Include("PicturePanel").Include("VideoTable").Include("CombomPanel").Include("TextPanel").Include("PanelStyle").Include("GalleryPanel").Include("GalleryPanel.GalleryImages")
                                    where b.PageSetting.Id == this.Style.ThisPageId
                                    orderby b.Panel_Id
                                    select b;

                        foreach (var item in query)
                        {
                            this.Pan.Add(item);
                        }

                        HttpContext.Current.Cache[this.Style.ThisPageId + "content"] = this.Pan;
                    }
                    else
                    {
                        this.Pan = panels;
                    }
                }
            }
        }

        public void GetIndexing()
        {
            using (this.ef = new CMSDbEntities())
            {
                if (this.Pan == null)
                {
                    this.Pan = new Collection<Panel>();
                    Collection<Panel> panels = HttpContext.Current.Cache[this.Style.ThisPageId + "content"] as Collection<Panel>;
                    if (panels == null)
                    {
                        var query = from b in this.ef.Panels.Include("PicturePanel").Include("VideoTable").Include("CombomPanel").Include("TextPanel").Include("PanelStyle").Include("GalleryPanel").Include("GalleryPanel.GalleryImages").Where(x => x.PageSettingId == this.Style.ThisPageId).OrderBy(t => t.MenuIndex).ThenBy(t => t.UnderMenuIndex)
                                    where b.PageSettingId == this.Style.ThisPageId
                                    select b;

                        foreach (var item in query)
                        {
                            this.Pan.Add(item);
                        }

                        HttpContext.Current.Cache[this.Style.ThisPageId + "content"] = this.Pan;
                    }
                    else
                    {
                        this.Pan = panels;
                    }
                }
            }
        }
    }
}