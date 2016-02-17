using System;
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
            CreateFoldersIfNeeded();
        }

        public Stream GetFile(string fileName)
        {
            return GetAnyFile(_fileStorageSettings.FileStorageFolder, fileName);
        }

        public Stream GetImage(string imageName)
        {
            return GetAnyFile(_fileStorageSettings.ImageStorageFolder, imageName);
        }

        public async Task<string> UploadFileAsync(HttpContent content)
        {
            Require.NotNull(content, nameof(content));
            var filePath = await UploadAnyFileAsync(
                content, 
                _fileStorageSettings.AllowedFileExtensions, 
                _fileStorageSettings.FileStorageFolder);
            var fileName = Path.GetFileName(filePath);
            var newFileName = SaltFileNameWithCurrentDate(fileName);
            var newFilePath = Path.Combine(_fileStorageSettings.FileStorageFolder, newFileName);
            RenameFile(filePath, newFilePath);
            return newFileName;
        }

        public async Task<string> UploadImageAsync(HttpContent content)
        {
            Require.NotNull(content, nameof(content));
            var filePath = await UploadAnyFileAsync(
                content,
                _fileStorageSettings.AllowedImageExtensions,
                _fileStorageSettings.ImageStorageFolder);
            var fileName = Path.GetFileName(filePath);
            var newFileName = GenerateRandomFileName(fileName);
            var newFilePath = Path.Combine(_fileStorageSettings.ImageStorageFolder, newFileName);
            RenameFile(filePath, newFilePath);
            return newFileName;
        }

        private Stream GetAnyFile(string folderPath, string fileName)
        {
            var fullPath = Path.Combine(folderPath, fileName);
            var exists = File.Exists(fullPath);
            if (!exists)
            {
                throw new FileNotFoundException();
            }

            return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
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
            var fullFileName = provider.FileData.First().LocalFileName;
            var extension = GetFileExtension(fullFileName);
            if (!allowedExtensions.Contains(extension))
            {
                await Task.Factory.StartNew(() => File.Delete(fullFileName));
                throw new InvalidDataException("Extension {0} is not allowed".FormatString(extension)); 
            }

            return fullFileName;
        }

        private string GetFileExtension(string fileName)
        {
            return Path.GetExtension(fileName).TrimStart('.');
        }

        private void CreateFoldersIfNeeded()
        {
            if (!Directory.Exists(_fileStorageSettings.FileStorageFolder))
            {
                Directory.CreateDirectory(_fileStorageSettings.FileStorageFolder);
            }

            if (!Directory.Exists(_fileStorageSettings.ImageStorageFolder))
            {
                Directory.CreateDirectory(_fileStorageSettings.ImageStorageFolder);
            }
        }

        private string GenerateRandomFileName(string fileName)
        {
            var randomFileName = Path.GetRandomFileName();
            return Path.ChangeExtension(randomFileName, GetFileExtension(fileName));
        }

        private string SaltFileNameWithCurrentDate(string fileName)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var newFileName = fileNameWithoutExtension 
                + DateTime.Now.ToString("s").Replace(":", string.Empty) 
                + Path.GetExtension(fileName);
            return newFileName;
        }

        private void RenameFile(string originalFullName, string newFileFullName)
        {
            File.Move(originalFullName, newFileFullName);
        }

        private readonly FileStorageSettings _fileStorageSettings;
    }
}
