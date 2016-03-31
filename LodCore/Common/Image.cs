using System;

namespace Common
{
    public class Image
    {
        public Image(Uri bigPhotoUri, Uri smallPhotoUri)
        {
            BigPhotoUri = bigPhotoUri;
            SmallPhotoUri = smallPhotoUri;
        }

        public static implicit operator Uri(Image image)
        {
            return image.SmallPhotoUri;
        }
        
        public virtual Uri BigPhotoUri { get; set; }
        public virtual Uri SmallPhotoUri { get; set; }
    }
}