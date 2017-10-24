namespace NextStepMVC.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Models;

    public class AddController : Controller
    {
        private RetrieveContent rc;
        private PanelManagement pm;

        public AddController()
        {
            this.rc = new RetrieveContent();
            this.rc.GetIndexing();
        }

        public ActionResult GetMenuTags(string term)
        {
            List<string> tags = new List<string>();
            using (var ef = new CMSDbEntities())
            {
                var query = from b in ef.Panels
                    orderby b.MenuIndex
                    where b.PageSettingId == this.rc.Style.ThisPageId
                    select b;
                var index = 0;
                foreach (var item in query)
                {
                    if (item.MenuIndex > index)
                    {
                        tags.Add(item.MenuTag);
                        index = Convert.ToInt32(item.MenuIndex, CultureInfo.CurrentCulture);
                    }
                }
            }

            return this.Json(tags.Where(t => t.StartsWith(term)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddTextPanel()
        {
            return this.View(this.rc);
        }

        public ActionResult AddPicturePanel()
        {
            return this.View(this.rc);
        }

        public ActionResult AddVideoPanel()
        {
            return this.View(this.rc);
        }

        public ActionResult AddComboPanel()
        {
            return this.View(this.rc);
        }

        public ActionResult AddStyle()
        {
            return this.View(this.rc);
        }

        public ActionResult AddGalleryPanel()
        {
            return this.View(this.rc);
        }

        [HttpPost]
        public ActionResult AddGalleryPanel(FormCollection collection, IEnumerable<HttpPostedFileBase> files)
        {
            if (files != null && collection != null)
            {
                this.rc = new RetrieveContent();
                switch (collection["submit"])
                {
                    case "save":
                        var menuTag = collection["menuTag"];
                        var styleSheet = Convert.ToInt32(collection["panelDesign"], CultureInfo.CurrentCulture);

                        var header = collection["hdr"];
                        var underMenuTag = collection["underMenuTag"];

                        this.pm = new PanelManagement();
                        this.pm.AddPanel(styleSheet, menuTag, underMenuTag);
                        this.rc.ClearPanelCache();
                        this.rc.GetContent();
                        var id = this.rc.Pan.Last().Panel_Id;
                        this.pm.AddGalleryPanel(id, header);

                        foreach (var item in files)
                        {
                            if (HttpPostedFileBaseExtensions.IsImage(item))
                            {
                                this.pm.AddGalleryImage(id, item);
                            }
                        }

                        this.rc.ClearPanelCache();
                        return this.RedirectToAction("AdminHome", "Admin");

                    case "cancel":
                        return this.RedirectToAction("AdminHome", "Admin");
                    default:
                        throw new ArgumentNullException("collection");
                }
            }

            return this.RedirectToAction("AdminLogOn", "Admin");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddTextPanel(FormCollection collection)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin" && collection != null)
            {
                this.rc = new RetrieveContent();
                switch (collection["submit"])
                {
                    case "save":
                        var menuTag = collection["menuTag"];
                        var styleSheet = Convert.ToInt32(collection["panelDesign"], CultureInfo.CurrentCulture);

                        var header = collection["hdr"];
                        var context = collection["cntxt"];
                        var underMenuTag = collection["underMenuTag"];

                        this.pm = new PanelManagement();
                        this.pm.AddPanel(styleSheet, menuTag, underMenuTag);
                        this.rc.ClearPanelCache();
                        this.rc.GetContent();

                        var thispanel = this.rc.Pan.Last();
                        this.pm.AddTextPanel(thispanel.Panel_Id, context, header);

                        this.rc.ClearPanelCache();
                        return this.RedirectToAction("AdminHome", "Admin");

                    case "cancel":
                        return this.RedirectToAction("AdminHome", "Admin");
                    default:
                        throw new ArgumentNullException("collection");
                }
            }

            return this.RedirectToAction("AdminLogOn", "Admin");
        }

        [HttpPost]
        public ActionResult AddVideoPanel(FormCollection collection)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin" && collection != null)
            {
                this.rc = new RetrieveContent();
                switch (collection["submit"])
                {
                    case "save":
                        var menuTag = collection["menuTag"];
                        var styleSheet = Convert.ToInt32(collection["panelDesign"], CultureInfo.CurrentCulture);

                        var header = collection["hdr"];
                        var videoUrl = collection["videoURL"];
                        var underMenuTag = collection["underMenuTag"];

                        this.pm = new PanelManagement();
                        this.pm.AddPanel(styleSheet, menuTag, underMenuTag);
                        this.rc.ClearPanelCache();
                        this.rc.GetContent();

                        var thispanel = this.rc.Pan.Last();
                        this.pm.AddVideoPanel(thispanel.Panel_Id, videoUrl, header);

                        this.rc.ClearPanelCache();
                        return this.RedirectToAction("AdminHome", "Admin");

                    case "cancel":
                        return this.RedirectToAction("AdminHome", "Admin");
                    default:
                        throw new ArgumentNullException("collection");
                }
            }
            else
            {
                return this.RedirectToAction("AdminLogOn", "Admin");
            }
        }

        [HttpPost]
        public ActionResult AddPicturePanel(FormCollection collection)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin" && collection != null)
            {
                this.rc = new RetrieveContent();
                switch (collection["submit"])
                {
                    case "save":
                        var menuTag = collection["menuTag"];
                        var styleSheet = Convert.ToInt32(collection["panelDesign"], CultureInfo.CurrentCulture);

                        var header = collection["hdr"];
                        var imagefile = this.Request.Files["imagefile"];
                        var underMenuTag = collection["underMenuTag"];

                        this.pm = new PanelManagement();
                        this.pm.AddPanel(styleSheet, menuTag, underMenuTag);
                        this.rc.ClearPanelCache();
                        this.rc.GetContent();
                        var thispanel = this.rc.Pan.Last();
                        this.pm.AddPicturePanel(thispanel.Panel_Id, header, imagefile);

                        this.rc.ClearPanelCache();
                        return this.RedirectToAction("AdminHome", "Admin");

                    case "cancel":
                        return this.RedirectToAction("AdminHome", "Admin");
                    default:
                        throw new ArgumentNullException("collection");
                }
            }
            else
            {
                return this.RedirectToAction("AdminLogOn", "Admin");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddComboPanel(FormCollection collection)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin" && collection != null)
            {
                this.rc = new RetrieveContent();
                switch (collection["submit"])
                {
                    case "save":
                        var menuTag = collection["menuTag"];
                        var styleSheet = Convert.ToInt32(collection["panelDesign"], CultureInfo.CurrentCulture);

                        var header = collection["hdr"];
                        var pictureAlign = collection["align"];
                        var context = collection["cntxt"];
                        var imagefile = this.Request.Files["imagefile"];
                        var underMenuTag = collection["underMenuTag"];

                        this.pm = new PanelManagement();
                        this.pm.AddPanel(styleSheet, menuTag, underMenuTag);

                        this.rc.ClearPanelCache();
                        this.rc.GetContent();

                        var thispanel = this.rc.Pan.Last();
                        this.pm.AddComboPanel(thispanel.Panel_Id, header, context, pictureAlign, imagefile);

                        this.rc.ClearPanelCache();
                        return this.RedirectToAction("AdminHome", "Admin");

                    case "cancel":
                        return this.RedirectToAction("AdminHome", "Admin");
                    default:
                        throw new ArgumentNullException("collection");
                }
            }
            else
            {
                return this.RedirectToAction("AdminLogOn", "Admin");
            }
        }

        [HttpPost]
        public ActionResult AddStyle(FormCollection collection)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin" && collection != null)
            {
                var titleColor = collection["titleColor"];
                var contextColor = collection["contextColor"];
                var panelColor = collection["backgroundColor"];
                var titleFont = collection["titleFont"];
                var contextFont = collection["contextFont"];
                var styleName = collection["styleName"];

                var ps = new PanelStyle()
                {
                    StyleName = styleName,
                    color_panel_background = panelColor,
                    color_panel_context = contextColor,
                    color_panel_title = titleColor,
                    font_family_panel_context = contextFont,
                    font_family_panel_title = titleFont,
                    PageSettingId = this.rc.Style.ThisPageId
                };
                var psm = new PanelStyleManagement();
                psm.AddPanelStyle(ps);

                this.rc.ClearStyleCache();
                return this.RedirectToAction("AdminHome", "Admin");
            }
            else
            {
                return this.RedirectToAction("AdminLogOn", "Admin");
            }
        }
    }
}
