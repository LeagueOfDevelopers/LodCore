using System;

namespace LodCoreLibrary.Infrastructure.FilesManagement
{
    public interface IImageResizer
    {
        Uri ResizeImageByLengthOfLongestSide(Uri imageToResizeUri);
    }
}