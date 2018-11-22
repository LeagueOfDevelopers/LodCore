using System;

namespace LodCoreLibraryOld.Infrastructure.FilesManagement
{
    public interface IImageResizer
    {
        Uri ResizeImageByLengthOfLongestSide(Uri imageToResizeUri);
    }
}