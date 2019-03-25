using Journalist;

namespace LodCore.Infrastructure.FilesManagement
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

            FileStorageFolder = fileStorageFolder;
            AllowedFileExtensions = allowedFileExtensions;
            ImageStorageFolder = imageStorageFolder;
            AllowedImageExtensions = allowedImageExtensions;
        }

        //public string FileStorageFolder => HttpContext.Current.Server.MapPath(_fileStoragePath);
        public string FileStorageFolder { get; }

        public string[] AllowedFileExtensions { get; }

        //public string ImageStorageFolder => HttpContext.Current.Server.MapPath(_imageStoragePath);
        public string ImageStorageFolder { get; }

        public string[] AllowedImageExtensions { get; }
    }
}