using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
            if (!httpContent.IsMimeMultipartContent("form-data"))
            {
                throw new NotSupportedException();
            }

            var provider = new CustomMultipartStreamProvider(folderPath);
            await httpContent.ReadAsMultipartAsync(provider);
            var fileName = provider.FileData.First().LocalFileName;
            var extension = GetFileExtension(fileName);
            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidDataException("Extension {0} is not allowed".FormatString(extension)); 
            }

            return fileName;
        }

        private string GetFileExtension(string fileName)
        {
            return Path.GetExtension(fileName).TrimStart('.');
        }

        private readonly FileStorageSettings _fileStorageSettings;
    }
}
