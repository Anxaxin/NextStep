namespace NextStepMVC.Models
{
    public class ImageFile
    {
        public byte[] ImageFileValue { get; set; }

        public string ImageType { get; set; }

        public void ImageCombo(int id)
        {
            using (var ef = new CMSDbEntities())
            {
                var entity = ef.CombomPanels.Find(id);
                if (entity == null)
                {
                    return;
                }

                this.ImageFileValue = entity.ImageData;
                this.ImageType = entity.ImageType;
            }
        }

        public void ImagePanel(int id)
        {
            using (var ef = new CMSDbEntities())
            {
                var entity = ef.PicturePanels.Find(id);
                if (entity == null)
                {
                    return;
                }

                this.ImageFileValue = entity.ImageData;
                this.ImageType = entity.ImageType;
            }
        }

        public void ImageGallery(int id)
        {
            using (var ef = new CMSDbEntities())
            {
                var entity = ef.ImageFullSizes.Find(id);
                if (entity == null)
                {
                    return;
                }

                this.ImageFileValue = entity.imagedata;
                this.ImageType = entity.imagetype;
            }
        }

        public void ImageGalleryThumb(int id)
        {
            using (var ef = new CMSDbEntities())
            {
                var entity = ef.GalleryImages.Find(id);
                if (entity == null)
                {
                    return;
                }

                this.ImageFileValue = entity.imageThumb;
                this.ImageType = entity.imageType;
            }
        }
    }
}