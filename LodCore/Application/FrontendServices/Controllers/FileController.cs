using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

        [HttpGet]
        [Route("file/{filename}")]
        public HttpResponseMessage GetFile(string filename)
        {
            Stream stream;
            try
            {
                stream = _fileManager.GetFile(filename);
            }
            catch (FileNotFoundException)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(stream)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return response;
        }

        [HttpPost]
        [Route("file")]
        public async Task<string> UploadFile()
        {
            try
            {
                return await _fileManager.UploadFileAsync(Request.Content);
            }
            catch (NotSupportedException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            catch (InvalidDataException)
            {
                throw new HttpResponseException(HttpStatusCode.NotAcceptable);
            }
        }

        [HttpPost]
        [Route("image")]
        public async Task<string> UploadImage()
        {
            try
            {
                return await _fileManager.UploadImageAsync(Request.Content);
            }
            catch (NotSupportedException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            catch (InvalidDataException)
            {
                throw new HttpResponseException(HttpStatusCode.NotAcceptable);
            }
        }

        private readonly IFileManager _fileManager;
    }
}
