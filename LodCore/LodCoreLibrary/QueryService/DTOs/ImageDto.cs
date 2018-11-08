using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LodCoreLibrary.Common;

namespace LodCoreLibrary.QueryService.DTOs
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
