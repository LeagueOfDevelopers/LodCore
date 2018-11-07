using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.QueryService.DTOs
{
    public class ImageDto
    {
        public ImageDto()
        {
        }

        public int ProjectId { get; set; }
        public string BigPhotoUri { get; set; }
        public string SmallPhotoUri { get; set; }
    }
}
