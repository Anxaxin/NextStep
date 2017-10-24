// <copyright file="ImageController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NextStepMVC.Controllers
{
    using System.Web.Mvc;
    using Models;

    public class ImageController : Controller
    {
        public ActionResult ShowCombo(int id)
        {
            var rc = new ImageFile();
            rc.ImageCombo(id);
            var imageData = rc.ImageFileValue;
            var imageType = rc.ImageType;

            return this.File(imageData, imageType);
        }

        public ActionResult ShowImage(int id)
        {
            var rc = new ImageFile();
            rc.ImagePanel(id);
            var imageData = rc.ImageFileValue;
            var imageType = rc.ImageType;

            return this.File(imageData, imageType);
        }

        public ActionResult ShowGalleryImage(int id)
        {
            var rc = new ImageFile();
            rc.ImageGallery(id);
            if (rc.ImageFileValue != null && rc.ImageType != null)
            {
                var imageData = rc.ImageFileValue;
                var imageType = rc.ImageType;
                return this.File(imageData, imageType);
            }
            else
            {
                return this.Redirect(this.Request.UrlReferrer.ToString());
            }
        }

        public ActionResult ShowGalleryThumb(int id)
        {
            var rc = new ImageFile();
            rc.ImageGalleryThumb(id);
            if (rc.ImageFileValue != null && rc.ImageType != null)
            {
                var imageData = rc.ImageFileValue;
                var imageType = rc.ImageType;
                return this.File(imageData, imageType);
            }
            else
            {
                return this.Redirect(this.Request.UrlReferrer.ToString());
            }
        }

        public ActionResult DeleteGalleryImage(int id)
        {
            using (var ef = new CMSDbEntities())
            {
                var image = ef.GalleryImages.Find(id);
                if (image != null)
                {
                    ef.ImageFullSizes.Remove(image.ImageFullSize);
                    ef.GalleryImages.Remove(image);
                    ef.SaveChanges();
                }
            }

            return this.Redirect(this.Request.UrlReferrer.ToString());
        }
    }
}