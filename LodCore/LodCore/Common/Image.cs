using System;

namespace LodCore.Common
{
    public class Image
    {
        public Image()
        {
            
        }

        public Image(Uri bigPhotoUri, Uri smallPhotoUri)
        {
            BigPhotoUri = bigPhotoUri;
            SmallPhotoUri = smallPhotoUri;
        }
        
        public virtual Uri BigPhotoUri { get; set; }
        public virtual Uri SmallPhotoUri { get; set; }
    }
}