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

            var magicImage = new MagickImage(Path.Combine(fileStorageSettings.ImageStorageFolder, fileName));

            var size = magicImage.Height >= magicImage.Width
                ? new MagickGeometry(lengthOfLongestSideOfResized, 0)
                : new MagickGeometry(0, lengthOfLongestSideOfResized);

            size.IgnoreAspectRatio = false;

            magicImage.Resize(size);

            var pathOfNewImage = Path.Combine(fileStorageSettings.ImageStorageFolder, GenerateRandomFileName(magicImage.FileName));
            magicImage.Write(pathOfNewImage);

            return new Uri(pathOfNewImage);
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
        private readonly FileStorageSettings fileStorageSettings;

        public ImageResizer(int lengthOfLongestSideOfResized, FileStorageSettings fileStorageSettings)
        {
            _lengthOfLongestSideOfResized = lengthOfLongestSideOfResized;
            this.fileStorageSettings = fileStorageSettings;
        }
    }
}