using System;

namespace LodCore.Infrastructure.FilesManagement
{
    public interface IImageResizer
    {
        Uri ResizeImageByLengthOfLongestSide(Uri imageToResizeUri);
    }
}