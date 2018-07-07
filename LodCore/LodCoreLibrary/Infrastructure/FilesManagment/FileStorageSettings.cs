using System.IO;
using System.Web;
using Journalist;

namespace LodCoreLibrary.Infrastructure.FilesManagement
{
    public class FileStorageSettings
    {
        public FileStorageSettings(
            string fileStorageFolder, 
            string[] allowedFileExtensions, 
            string imageStorageFolder, 
            string[] allowedImageExtensions)
        {
            Require.NotEmpty(fileStorageFolder, nameof(fileStorageFolder));
            Require.NotNull(allowedFileExtensions, nameof(allowedFileExtensions));
            Require.NotEmpty(imageStorageFolder, nameof(imageStorageFolder));
            Require.NotNull(allowedImageExtensions, nameof(allowedImageExtensions));

            _fileStoragePath = fileStorageFolder;
            AllowedFileExtensions = allowedFileExtensions;
            _imageStoragePath = imageStorageFolder;
            AllowedImageExtensions = allowedImageExtensions;
        }

        public string FileStorageFolder => HttpContext.Current.Server.MapPath(_fileStoragePath);

        public string[] AllowedFileExtensions { get; private set; }

        public string ImageStorageFolder => HttpContext.Current.Server.MapPath(_imageStoragePath);
        
        public string[] AllowedImageExtensions { get; private set; }

        private readonly string _fileStoragePath;

        private readonly string _imageStoragePath;
    }
}