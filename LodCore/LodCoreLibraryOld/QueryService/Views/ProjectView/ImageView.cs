﻿namespace LodCoreLibrary.QueryService.Views.ProjectView
{
    public class ImageView
    {
        public ImageView(string bigPhotoUri, string smallPhotoUri)
        {
            BigPhotoUri = bigPhotoUri;
            SmallPhotoUri = smallPhotoUri;
        }

        public string BigPhotoUri { get; set; }
        public string SmallPhotoUri { get; set; }
    }
}