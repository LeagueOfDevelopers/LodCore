using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Journalist;
using Journalist.Extensions;

namespace FilesManagement
{
    public class FileManager : IFileManager
    {
        public FileManager(FileStorageSettings fileStorageSettings)
        {
            Require.NotNull(fileStorageSettings, nameof(fileStorageSettings));

            _fileStorageSettings = fileStorageSettings;
        }

        public Task<string> UploadFileAsync(HttpContent content)
        {
            Require.NotNull(content, nameof(content));
            return UploadAnyFileAsync(
                content, 
                _fileStorageSettings.AllowedFileExtensions, 
                _fileStorageSettings.FileStorageFolder);
        }

        public Task<string> UploadImageAsync(HttpContent content)
        {
            Require.NotNull(content, nameof(content));
            return UploadAnyFileAsync(
                content,
                _fileStorageSettings.AllowedImageExtensions,
                _fileStorageSettings.ImageStorageFolder);
        }

        private async Task<string> UploadAnyFileAsync(
            HttpContent httpContent, 
            string[] allowedExtensions, 
            string folderPath)
        {
            var fileName = GetFileName(httpContent.Headers);
            if (fileName == null)
            {
                throw new ArgumentNullException("httpContent.Headers");
            }

            var fileExtension = GetFileExtension(fileName);
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new InvalidDataException("Extension {0} is not allowed".FormatString(fileExtension));
            }

            var multipartStreamProvider = new MultipartFormDataStreamProvider(folderPath);
            await httpContent.ReadAsMultipartAsync(multipartStreamProvider);
            return fileName;
        }

        private string GetFileName(HttpContentHeaders headers)
        {
            return headers?.ContentDisposition?.FileName?.Trim('"');
        }

        private string GetFileExtension(string fileName)
        {
            return Path.GetExtension(fileName);
        }

        private readonly FileStorageSettings _fileStorageSettings;
    }
}
