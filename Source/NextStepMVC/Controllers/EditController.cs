// <copyright file="EditController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NextStepMVC.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;
    using Models;

    public class EditController : Controller
    {
        private readonly RetrieveContent rc;

        public EditController()
        {
            this.rc = new RetrieveContent();
            this.rc.GetIndexing();
            this.rc.RetrievePanelStyles();
        }

        public ActionResult EditSiteStyle()
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin")
            {
                    return this.View(this.rc);
            }
            else
            {
                return this.RedirectToAction("AdminLogOn", "Admin");
            }
        }

        public ActionResult EditMenuIndexing()
        {
            this.rc.GetIndexing();
            return this.View(this.rc);
        }

        public ActionResult EditStyle(int? id)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin")
            {
                this.ViewBag.id = id;
                return this.View(this.rc);
            }
            else
            {
                return this.RedirectToAction("AdminLogOn", "Admin");
            }
        }

        public ActionResult EditComboPanel(int? id)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin")
            {
                this.ViewBag.id = id;
                this.rc.RetrievePanelStyles();
                return this.View(this.rc);
            }
            else
            {
                return this.RedirectToAction("AdminLogOn", "Admin");
            }
        }

        public ActionResult EditPicturePanel(int? id)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin")
            {
                this.ViewBag.id = id;
                return this.View(this.rc);
            }
            else
            {
                return this.RedirectToAction("AdminLogOn", "Admin");
            }
        }

        public ActionResult EditTextPanel(int? id)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin")
            {
                this.ViewBag.id = id;
                return this.View(this.rc);
            }
            else
            {
                return this.RedirectToAction("AdminLogOn", "Admin");
            }
        }

        public ActionResult EditVideoPanel(int? id)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin")
            {
                this.ViewBag.id = id;
                return this.View(this.rc);
            }
            else
            {
                return this.RedirectToAction("AdminLogOn", "Admin");
            }
        }

        public ActionResult EditGalleryPanel(int? id)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin")
            {
                this.ViewBag.id = id;
                return this.View(this.rc);
            }
            else
            {
                return this.RedirectToAction("AdminLogOn", "Admin");
            }
        }

        public ActionResult EditImageDescription(int? id)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin")
            {
                this.ViewBag.id = id;
                using (var ef = new CMSDbEntities())
                {
                    var entity = ef.GalleryImages.Find(id);
                    if (entity == null)
                    {
                        return this.RedirectToAction("AdminHome", "Admin");
                    }

                    this.ViewBag.imgtitle = entity.imageTitle;
                    this.ViewBag.imgdesc = entity.imageDescription;
                }

                    return this.View(this.rc);
            }
            else
            {
                return this.RedirectToAction("AdminLogOn", "Admin");
            }
        }

        public ActionResult MoveMenuIndexUp(int id)
        {
            var pm = new PanelManagement();
            pm.MovePanelIndexUp(id);

            this.rc.ClearPanelCache();
            return this.RedirectToAction("EditMenuIndexing", "Edit");
        }

        public ActionResult MoveMenuIndexDown(int id)
        {
            var pm = new PanelManagement();
            pm.MovePanelIndexDown(id);

            this.rc.ClearPanelCache();
            return this.RedirectToAction("EditMenuIndexing", "Edit");
        }

        public ActionResult MoveUnderMenuIndexUp(int index, int underIndex)
        {
            var pm = new PanelManagement();
            pm.MoveUnderMenuIndexUp(index, underIndex);

            this.rc.ClearPanelCache();
            return this.RedirectToAction("EditMenuIndexing", "Edit");
        }

        public ActionResult MoveUnderMenuIndexDown(int index, int underIndex)
        {
            var pm = new PanelManagement();
            pm.MoveUnderMenuIndexDown(index, underIndex);

            this.rc.ClearPanelCache();
            return this.RedirectToAction("EditMenuIndexing", "Edit");
        }

        [HttpPost]
        public ActionResult EditImageDescription(FormCollection collection)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin" && collection != null)
            {
                var pm = new PanelManagement();
                var thisId = Convert.ToInt32(collection["thisID"], CultureInfo.CurrentCulture);
                switch (collection["submit"])
                {
                    case "save":
                        var desc = collection["desc"];
                        var title = collection["hdr"];
                        var entity = pm.EditGalleryImage(thisId, desc, title);

                        this.rc.ClearPanelCache();
                        return this.RedirectToAction("EditGalleryPanel", "Edit", new { id = entity });

                    case "delete":
                            var delent = pm.DeleteGalleryImage(thisId);

                        this.rc.ClearPanelCache();
                        return this.RedirectToAction("EditGalleryPanel", "Edit", new { id = delent });
                    default:
                        throw new ArgumentNullException("collection");
                }
            }

                return this.View();
        }

        [HttpPost]
        public ActionResult EditGalleryPanel(FormCollection collection, IEnumerable<HttpPostedFileBase> files)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin" && files != null && collection != null)
            {
                var pm = new PanelManagement();
                var thisId = Convert.ToInt32(collection["thisID"], CultureInfo.CurrentCulture);
                switch (collection["submit"])
                {
                    case "save":
                        var styleSheet = Convert.ToInt32(collection["panelDesign"], CultureInfo.CurrentCulture);
                        var header = collection["hdr"];
                        var menuTag = collection["menuTag"];
                        var underMenuTag = collection["underMenuTag"];

                        pm.EditGalleryPanel(thisId, styleSheet, header, menuTag, underMenuTag);
                        foreach (var item in files)
                        {
                            if (item != null)
                            {
                                if (HttpPostedFileBaseExtensions.IsImage(item))
                                {
                                    pm.AddGalleryImage(thisId, item);
                                }
                            }
                        }

                        this.rc.ClearPanelCache();
                        return this.Redirect(this.Request.UrlReferrer.ToString());

                    case "cancel":
                        return this.RedirectToAction("AdminHome", "Admin");
                    case "delete":
                        pm.DeletePanel(thisId);

                        this.rc.ClearPanelCache();
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
        public ActionResult EditComboPanel(FormCollection collection)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin" && collection != null)
            {
                var pm = new PanelManagement();
                var thisId = Convert.ToInt32(collection["thisID"], CultureInfo.CurrentCulture);
                switch (collection["submit"])
                {
                    case "save":
                        var menuTag = collection["menuTag"];
                        var undermenutag = collection["underMenuTag"];
                        var styleSheet = Convert.ToInt32(collection["panelDesign"], CultureInfo.CurrentCulture);
                        var header = collection["hdr"];
                        var pictureAlign = collection["align"];
                        var context = collection["cntxt"];
                        var imagefile = this.Request.Files["imagefile"];
                        pm.EditComboPanel(thisId, menuTag, undermenutag, styleSheet, imagefile, pictureAlign, header, context);

                        this.rc.ClearPanelCache();
                        return this.RedirectToAction("AdminHome", "Admin");

                    case "cancel":
                        return this.RedirectToAction("AdminHome", "Admin");
                    case "delete":
                        pm.DeletePanel(thisId);

                        this.rc.ClearPanelCache();
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
        public ActionResult EditSiteStyle(FormCollection collection)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin" && collection != null)
            {
                switch (collection["submit"])
                {
                    case "Save":
                        var colorHeaderText = collection["menuTextColor"];
                        var colorHeaderTitle = collection["titleTextColor"];
                        var colorHeaderBackground = collection["menuColor"];
                        var colorSiteBackground = collection["backgroundColor"];
                        var fontHeaderTitle = collection["titleFont"];
                        var fontHeaderMenu = collection["menuFont"];
                        var pageName = collection["pageName"];
                        var imagefile = this.Request.Files["imagefile"];
                        var singlePage = Convert.ToBoolean(collection["singlePage"], CultureInfo.CurrentCulture);
                        var headerSize = Convert.ToInt32(collection["headerSize"], CultureInfo.CurrentCulture);

                        var headerPlacement = Convert.ToInt32(collection["AlignLogoRight"], CultureInfo.CurrentCulture);

                        var sss = new SiteStyleSheet()
                        {
                            color_header_background = colorHeaderBackground,
                            color_header_text = colorHeaderText,
                            color_header_title = colorHeaderTitle,
                            color_site_background = colorSiteBackground,
                            font_family_header = fontHeaderTitle,
                            font_family_header_menu = fontHeaderMenu
                        };

                        this.rc.Style.UpdateSiteStyle(sss, pageName, singlePage, imagefile, headerSize, headerPlacement);

                        this.rc.ClearPanelCache();
                        this.rc.ClearStyleCache();
                        return this.RedirectToAction("EditSiteStyle", "Edit");
                    case "Cancel":
                        return this.RedirectToAction("AdminHome", "Admin");
                    case "deleteLogo":
                        this.rc.Style.RemoveHeaderLogo();

                        this.rc.ClearPanelCache();
                        return this.RedirectToAction("EditSiteStyle", "Edit");
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
        public ActionResult EditStyle(FormCollection collection)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin" && collection != null)
            {
                var id = Convert.ToInt32(collection["thisId"], CultureInfo.CurrentCulture);
                var psm = new PanelStyleManagement();
                switch (collection["submit"])
                {
                    case "Save":
                        var titleColor = collection["titleColor"];
                        var contextColor = collection["contextColor"];
                        var panelColor = collection["backgroundColor"];
                        var titleFont = collection["titleFont"];
                        var contextFont = collection["contextFont"];
                        var styleName = collection["styleName"];

                        var newStyle = new PanelStyle()
                        {
                            color_panel_background = panelColor,
                            color_panel_context = contextColor,
                            color_panel_title = titleColor,
                            font_family_panel_context = contextFont,
                            font_family_panel_title = titleFont,
                            StyleName = styleName
                        };
                        psm.UpdatePanelStyle(newStyle, id);

                        this.rc.ClearStyleCache();
                        return this.RedirectToAction("AdminHome", "Admin");

                    case "Delete":
                        psm.DeletePanelStyle(id);

                        this.rc.ClearStyleCache();
                        return this.RedirectToAction("AdminHome", "Admin");
                    case "Cancel":
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
        public ActionResult EditPicturePanel(FormCollection collection)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin" && collection != null)
            {
                PanelManagement pm = new PanelManagement();

                var thisId = Convert.ToInt32(collection["thisID"], CultureInfo.CurrentCulture);
                switch (collection["submit"])
                {
                    case "save":
                        var menuTag = collection["menuTag"];
                        var undermenutag = collection["underMenuTag"];
                        var styleSheet = Convert.ToInt32(collection["panelDesign"], CultureInfo.InvariantCulture);

                        var header = collection["hdr"];
                        var imagefile = this.Request.Files["imagefile"];

                        pm.EditPicturePanel(thisId, menuTag, undermenutag, styleSheet, imagefile, header);

                        this.rc.ClearPanelCache();
                        return this.RedirectToAction("AdminHome", "Admin");

                    case "cancel":
                        return this.RedirectToAction("AdminHome", "Admin");
                    case "delete":
                        pm.DeletePanel(thisId);

                        this.rc.ClearPanelCache();
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
        public ActionResult EditTextPanel(FormCollection collection)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin" && collection != null)
            {
                var pm = new PanelManagement();

                var thisId = Convert.ToInt32(collection["thisID"], CultureInfo.CurrentCulture);
                switch (collection["submit"])
                {
                    case "save":
                        var menuTag = collection["menuTag"];
                        var undermenutag = collection["underMenuTag"];
                        var styleSheet = Convert.ToInt32(collection["panelDesign"], CultureInfo.CurrentCulture);
                        var header = collection["hdr"];
                        var context = collection["cntxt"];

                        pm.EditTextPanel(thisId, menuTag, undermenutag, styleSheet, header, context);

                        this.rc.ClearPanelCache();
                        return this.RedirectToAction("AdminHome", "Admin");

                    case "cancel":
                        return this.RedirectToAction("AdminHome", "Admin");
                    case "delete":
                        pm.DeletePanel(thisId);

                        this.rc.ClearPanelCache();
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
        public ActionResult EditVideoPanel(FormCollection collection)
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin")
            {
                var pm = new PanelManagement();
                if (collection != null)
                {
                    var thisId = Convert.ToInt32(collection["thisID"], CultureInfo.CurrentCulture);
                    switch (collection["submit"])
                    {
                        case "save":
                            var menuTag = collection["menuTag"];
                            var undermenutag = collection["underMenuTag"];
                            var styleSheet = Convert.ToInt32(collection["panelDesign"], CultureInfo.CurrentCulture);
                            var header = collection["hdr"];
                            var url = collection["videoURL"];

                            pm.EditVideoPanel(thisId, menuTag, undermenutag, styleSheet, url, header);

                            this.rc.ClearPanelCache();
                            return this.RedirectToAction("AdminHome", "Admin");

                        case "cancel":
                            return this.RedirectToAction("AdminHome", "Admin");
                        case "delete":
                            pm.DeletePanel(thisId);

                            this.rc.ClearPanelCache();
                            return this.RedirectToAction("AdminHome", "Admin");
                        default:
                            throw new ArgumentNullException("collection");
                    }
                }
                else
                {
                throw new ArgumentNullException("collection");
                }
            }
            else
            {
                return this.RedirectToAction("AdminLogOn", "Admin");
            }
        }
    }
}