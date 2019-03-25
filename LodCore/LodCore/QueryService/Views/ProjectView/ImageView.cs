using System;

namespace LodCore.QueryService.Views.ProjectView
{
    public class ImageView : IEquatable<ImageView>
    {
        public ImageView(string bigPhotoUri, string smallPhotoUri)
        {
            BigPhotoUri = bigPhotoUri;
            SmallPhotoUri = smallPhotoUri;
        }

        public string BigPhotoUri { get; set; }
        public string SmallPhotoUri { get; set; }

        public bool Equals(ImageView obj)
        {
            return BigPhotoUri == obj.BigPhotoUri && SmallPhotoUri == obj.SmallPhotoUri;
        }
    }
}