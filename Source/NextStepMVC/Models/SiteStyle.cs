namespace NextStepMVC.Models
{
    using System;
    using System.Web;
    using System.Web.Helpers;

    public class SiteStyle
    {
        private CMSDbEntities ef;

        public SiteStyle()
        {
            using (this.ef = new CMSDbEntities())
            {
                var style = this.ef.SiteStyleSheets.Find(this.ef.PageSettings.Find(Settings.GetCurrentDomain().PageSettingId).pageStyle);
                var settings = this.ef.PageSettings.Find(Settings.GetCurrentDomain().PageSettingId);

                this.NextStepUrl = Settings.GetCurrentDomain().NextStepUrl;
                this.PageTitle = settings.PageName;
                this.Footertext = settings.FooterText;
                this.SinglePage = settings.singlePage;
                this.ThisPageId = settings.Id;
                this.HeaderSize = settings.HeaderSize;
                this.HeaderAlignRight = settings.AlignHeaderRight;

                if (settings.HeaderImage != null)
                {
                    var wi = new WebImage(settings.HeaderImage);
                    var height = wi.Height;
                    var maxsize = (settings.HeaderSize * 2) + 20;
                    var ratio = (double)maxsize / (double)height;
                    if (ratio != 0)
                    {
                        wi.Resize(Convert.ToInt32(wi.Width * ratio), maxsize);
                        this.ImageData = wi.GetBytes();
                    }
                }

                this.ColorHeaderText = style.color_header_text;
                this.ColorHeaderTitle = style.color_header_title;
                this.ColorHeaderBackground = style.color_header_background;
                this.FontHeaderTitle = style.font_family_header;
                this.FontHeaderMenu = style.font_family_header_menu;
                this.ColorSiteBackground = style.color_site_background;
            }
        }

        public int HeaderAlignRight { get; set; }

        public bool NextStepUrl { get; set; }

        public int ThisPageId { get; set; }

        public int HeaderSize { get; set; }

        public byte[] ImageData { get; set; }

        public string ColorHeaderText { get; set; }

        public string ColorHeaderTitle { get; set; }

        public string ColorHeaderBackground { get; set; }

        public string FontHeaderTitle { get; set; }

        public string FontHeaderMenu { get; set; }

        public string ColorSiteBackground { get; set; }

        public string PageTitle { get; set; }

        public string Footertext { get; set; }

        public bool SinglePage { get; set; }

        public void UpdateSiteStyle(SiteStyleSheet updated, string pagename, bool singlePage, HttpPostedFileBase thisFile, int headerSize, int headerAlign)
        {
            if (updated != null)
            {
                using (this.ef = new CMSDbEntities())
                {
                    var original = this.ef.SiteStyleSheets.Find(this.ef.PageSettings.Find(this.ThisPageId).SiteStyleSheet.Id);

                    original.color_header_background = updated.color_header_background;
                    original.color_header_text = updated.color_header_text;
                    original.color_header_title = updated.color_header_title;
                    original.color_site_background = updated.color_site_background;
                    original.font_family_header = updated.font_family_header;
                    original.font_family_header_menu = updated.font_family_header_menu;

                    var originalsetting = this.ef.PageSettings.Find(this.ThisPageId);
                    originalsetting.PageName = pagename;
                    originalsetting.singlePage = singlePage;
                    originalsetting.HeaderSize = headerSize;
                    originalsetting.AlignHeaderRight = headerAlign;

                    if (thisFile != null && thisFile.ContentLength > 100)
                    {
                        var lenght = thisFile.ContentLength;
                        var tempimage = new byte[lenght];
                        thisFile.InputStream.Read(tempimage, 0, lenght);

                        var wi = new WebImage(tempimage);
                        var height = wi.Height;
                        if (wi.Height > 60)
                        {
                            lel:
                            var ratio = (double)60 / (double)height;

                            if (ratio != 0)
                            {
                                wi.Resize(Convert.ToInt32(wi.Width * ratio), 60);
                                originalsetting.HeaderImage = wi.GetBytes();
                                originalsetting.HeaderImageType = thisFile.ContentType;
                            }
                            else
                            {
                                goto lel;
                            }
                        }
                        else
                        {
                            originalsetting.HeaderImage = tempimage;
                            originalsetting.HeaderImageType = thisFile.ContentType;
                        }
                    }

                    this.ef.SaveChanges();
                }
            }
        }

        public void RemoveHeaderLogo()
        {
            using (this.ef = new CMSDbEntities())
            {
                var page = this.ef.PageSettings.Find(this.ThisPageId);
                if (page == null)
                {
                    return;
                }

                page.HeaderImage = null;
                page.HeaderImageType = null;
                this.ef.SaveChanges();
            }
        }
    }
}