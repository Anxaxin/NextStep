// <copyright file="HttpPostedFileBaseExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NextStepMVC.Models
{
    using System.Globalization;
    using System.IO;
    using System.Web;

    public static class HttpPostedFileBaseExtensions
    {
        public const int ImageMinimumBytes = 512;

        public static bool IsImage(this HttpPostedFileBase postedFile)
        {
            if (postedFile == null)
            {
                return false;
            }

            if (postedFile.ContentType.ToLower(CultureInfo.CurrentCulture) != "image/jpg" &&
                            postedFile.ContentType.ToLower(CultureInfo.CurrentCulture) != "image/jpeg" &&
                            postedFile.ContentType.ToLower(CultureInfo.CurrentCulture) != "image/pjpeg" &&
                            postedFile.ContentType.ToLower(CultureInfo.CurrentCulture) != "image/gif" &&
                            postedFile.ContentType.ToLower(CultureInfo.CurrentCulture) != "image/x-png" &&
                            postedFile.ContentType.ToLower(CultureInfo.CurrentCulture) != "image/png")
            {
                return false;
            }

            if (Path.GetExtension(postedFile.FileName).ToLower(CultureInfo.CurrentCulture) != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower(CultureInfo.CurrentCulture) != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower(CultureInfo.CurrentCulture) != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower(CultureInfo.CurrentCulture) != ".jpeg")
            {
                return false;
            }

            return true;
        }
    }
}