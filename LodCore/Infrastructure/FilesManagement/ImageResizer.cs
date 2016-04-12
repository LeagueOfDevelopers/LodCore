using System;
using System.IO;
using System.Linq;
using ImageMagick;

namespace FilesManagement
{
    public class ImageResizer : IImageResizer
    {
        public Uri ResizeImageByLengthOfLongestSide(Uri imageToResizeUri, int lengthOfLongestSideOfResized)
        {
            var fileName = Path.GetFileName(imageToResizeUri.LocalPath);

            var magicImage = new MagickImage(Path.Combine(_fileStorageSettings.ImageStorageFolder, fileName));

            var size = magicImage.Height >= magicImage.Width
                ? new MagickGeometry(lengthOfLongestSideOfResized, 0)
                : new MagickGeometry(0, lengthOfLongestSideOfResized);

            size.IgnoreAspectRatio = false;

            magicImage.Resize(size);

            var newName = GenerateRandomFileName(magicImage.FileName);
            var pathOfNewImage = Path.Combine(_fileStorageSettings.ImageStorageFolder, newName);
            magicImage.Write(pathOfNewImage);

            return new Uri("http://api.lod-misis.ru/image/" + newName);
        }

        public int ReadLengthOfLongestSideOfResized()
        {
            return _lengthOfLongestSideOfResized;
        }

        private string GenerateRandomFileName(string fileName)
        {
            var randomFileName = Path.GetRandomFileName();
            return Path.ChangeExtension(randomFileName, Path.GetExtension(fileName).TrimStart('.'));
        }

        private readonly int _lengthOfLongestSideOfResized;
        private readonly FileStorageSettings _fileStorageSettings;

        public ImageResizer(int lengthOfLongestSideOfResized, FileStorageSettings fileStorageSettings)
        {
            _lengthOfLongestSideOfResized = lengthOfLongestSideOfResized;
            this._fileStorageSettings = fileStorageSettings;
        }
    }
}