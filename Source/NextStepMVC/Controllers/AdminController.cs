// <copyright file="AdminController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NextStepMVC.Controllers
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;
    using Models;

    public class AdminController : Controller
    {
        private readonly RetrieveContent rc;

        public AdminController()
        {
            this.rc = new RetrieveContent();
            this.rc.GetIndexing();
        }

        public ActionResult EditPass(int id)
        {
            this.rc.ListAdminUsers(this.rc.Style.ThisPageId);
            ViewBag.ThisId = id;
            return this.View(this.rc);
        }

        [HttpPost]
        public ActionResult EditPass(FormCollection collection)
        {
            if (collection != null)
            {
                var uid = Convert.ToInt32(collection["ThisId"], CultureInfo.CurrentCulture);
                ViewBag.ThisId = uid;
                var pwd = collection["pwd"];
                var npwd = collection["npwd"];
                var npwd2 = collection["nwpd2"];
                if (npwd == npwd2)
                { 
                    using (CMSDbEntities db = new CMSDbEntities())
                    {
                        var user = db.AdminLogins.Find(uid);
                        if (user.pass == pwd)
                        {
                            user.pass = npwd;
                            db.SaveChanges();
                            ViewBag.Success = "Password er nu ændret.";
                            this.rc.ListAdminUsers(this.rc.Style.ThisPageId);
                            return this.View(this.rc);
                        }
                        else
                        {
                            ViewBag.Error = "Nuværende password matcher ikke med det indtastede.";
                            this.rc.ListAdminUsers(this.rc.Style.ThisPageId);
                            return this.View(this.rc);
                        }
                    }
                }
                else
                {
                    ViewBag.Error = "De nye passwords stemmer ikke overens med hinanden.";
                    this.rc.ListAdminUsers(this.rc.Style.ThisPageId);
                    return this.View(this.rc);
                }
            }

            this.rc.ListAdminUsers(this.rc.Style.ThisPageId);
            return this.View(this.rc);
        }

        public ActionResult AdminLogOn()
        {
            return this.View(this.rc);
        }

        public ActionResult AdminHome()
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin")
            {
                this.rc.GetIndexing();
                return this.View(this.rc);
            }
            else
            {
                return this.RedirectToAction("AdminLogOn", "Admin");
            }
        }

        public ActionResult ManageUsers()
        {
            if (System.Web.HttpContext.Current.Session["admin"] != null && System.Web.HttpContext.Current.Session["admin"].ToString() == "iamadmin")
            {
                this.rc.ListAdminUsers(this.rc.Style.ThisPageId);
                return this.View(this.rc);
            }
            else
            {
                return this.RedirectToAction("AdminLogOn", "Admin");
            }
        }

        public ActionResult RemoveUser(int id)
        {
            AdminAccount.DeleteUser(id);
            ViewBag.Success = "Brugeren er nu slettet";

            return this.Redirect(this.Request.UrlReferrer.PathAndQuery);
        }

        public ActionResult EditUser(int id, string password, string pwd2, string newPassword)
        {
            if (password == pwd2)
            {
                AdminAccount.EditUser(id, newPassword);
                return this.Redirect(this.Request.UrlReferrer.PathAndQuery);
            }

            return this.Redirect(this.Request.UrlReferrer.PathAndQuery);
        }

        [HttpPost]
        public ActionResult ManageUsers(FormCollection collection)
        {
            if (collection != null)
            {
                var nusr = collection["nusr"];
                var npwd = collection["npwd"];
                var npwd2 = collection["npwd2"];

                if (npwd == npwd2)
                {
                    AdminAccount.AddUser(nusr, npwd, this.rc.Style.ThisPageId);
                    ViewBag.Success = "Brugeren er nu tilføjet";
                }
                else
                {
                    ViewBag.Error = "Password matcher ikke.";
                }
            }

            this.rc.ListAdminUsers(this.rc.Style.ThisPageId);
            return this.View(this.rc);
        }

        [HttpPost]
        public ActionResult AdminLogOn(FormCollection collection)
        {
            if (collection != null)
            {
                var usr = collection["usr"];
                var pwd = collection["pwd"];
                if (AdminAccount.LogOn(usr, pwd, this.rc.Style.ThisPageId))
                {
                    System.Web.HttpContext.Current.Session["admin"] = "iamadmin";
                    return this.RedirectToAction("AdminHome", "Admin");
                }
                else
                {
                    ViewBag.Error = "Dit brugernavn eller adgangskode er forkert.";
                    return this.View(this.rc);
                }
            }
            else
            {
                ViewBag.Error = "Der opstod en fejl, prøv venligst igen.";
                return this.View(this.rc);
            }
        }
    }
}