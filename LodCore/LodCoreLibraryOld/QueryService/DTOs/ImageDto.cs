using LodCoreLibraryOld.Common;

namespace LodCoreLibraryOld.QueryService.DTOs
{
    public class ImageDto
    {
        public ImageDto()
        {
        }

        public ImageDto(Image image, int projectId)
        {
            BigPhotoUri = image.BigPhotoUri.ToString();
            SmallPhotoUri = image.SmallPhotoUri.ToString();
            ProjectId = projectId;
        }

        public int ProjectId { get; set; }
        public string BigPhotoUri { get; set; }
        public string SmallPhotoUri { get; set; }
    }
}