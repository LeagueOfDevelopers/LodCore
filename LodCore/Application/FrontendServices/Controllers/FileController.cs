using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using FilesManagement;
using Journalist;

namespace FrontendServices.Controllers
{
    public class FileController : ApiController
    {
        public FileController(IFileManager fileManager)
        {
            Require.NotNull(fileManager, nameof(fileManager));

            _fileManager = fileManager;
        }

        [HttpPost]
        [Route("file")]
        public Task<string> UploadFile()
        {
            return _fileManager.UploadFileAsync(Request.Content);
        }

        [HttpPost]
        [Route("image")]
        public Task<string> UploadImage()
        {
            return _fileManager.UploadImageAsync(Request.Content);
        }

        private readonly IFileManager _fileManager;
    }
}
