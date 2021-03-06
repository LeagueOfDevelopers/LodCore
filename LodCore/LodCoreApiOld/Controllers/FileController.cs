﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Journalist;
using LodCoreApiOld.Authorization;
using LodCoreApiOld.Models;
using LodCoreLibraryOld.Domain.UserManagement;
using LodCoreLibraryOld.Infrastructure.FilesManagement;
using Serilog;

namespace LodCoreApiOld.Controllers
{
    public class FileController : ApiController
    {
        private readonly IFileManager _fileManager;

        public FileController(IFileManager fileManager)
        {
            Require.NotNull(fileManager, nameof(fileManager));

            _fileManager = fileManager;
        }

        [HttpGet]
        [Route("file/{fileName}")]
        [Authorization(AccountRole.User)]
        public HttpResponseMessage GetFile(string fileName)
        {
            return GetAnyFile(() => _fileManager.GetFile(fileName));
        }

        [HttpGet]
        [Route("image/{imageName}")]
        public HttpResponseMessage GetImage(string imageName)
        {
            return GetAnyFile(() => _fileManager.GetImage(imageName));
        }

        [HttpPost]
        [Route("file")]
        public async Task<string> UploadFile()
        {
            try
            {
                return await _fileManager.UploadFileAsync(Request.Content);
            }
            catch (NotSupportedException ex)
            {
                Log.Error("Failed to upload file with requets content headers={@0}. {1} StackTrace: {2}",
                    Request.Content.Headers, ex.Message, ex.StackTrace);
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            catch (InvalidDataException ex)
            {
                Log.Error("Failed to upload file with requets content headers={@0}. {1} StackTrace: {2}",
                    Request.Content.Headers, ex.Message, ex.StackTrace);
                throw new HttpResponseException(HttpStatusCode.NotAcceptable);
            }
        }

        [HttpPost]
        [Route("image")]
        [Authorization(AccountRole.User)]
        public async Task<Image> UploadImage()
        {
            try
            {
                var image = await _fileManager.UploadImageAsync(Request.Content);

                return new Image(Path.GetFileName(image.BigPhotoUri.LocalPath),
                    Path.GetFileName(image.SmallPhotoUri.LocalPath));
            }
            catch (NotSupportedException ex)
            {
                Log.Error("Failed to upload file with requets content headers={@0}. {1} StackTrace: {2}",
                    Request.Content.Headers, ex.Message, ex.StackTrace);
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            catch (InvalidDataException ex)
            {
                Log.Error("Failed to upload file with requets content headers={@0}. {1} StackTrace: {2}",
                    Request.Content.Headers, ex.Message, ex.StackTrace);
                throw new HttpResponseException(HttpStatusCode.NotAcceptable);
            }
        }

        private HttpResponseMessage GetAnyFile(Func<Stream> getStream)
        {
            Stream stream;
            try
            {
                stream = getStream();
            }
            catch (FileNotFoundException ex)
            {
                Log.Error("Failed to find file with requets content headers={@0}. {1} StackTrace: {2}",
                    Request.Content.Headers, ex.Message, ex.StackTrace);
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(stream)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return response;
        }
    }
}