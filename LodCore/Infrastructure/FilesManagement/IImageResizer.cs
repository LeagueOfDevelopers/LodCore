using System;

namespace FilesManagement
{
    public interface IImageResizer
    {
        Uri ResizeImageByLengthOfLongestSide(Uri imageToResizeUri, int lengthOfLongestSideOfResized);
        int ReadLengthOfLongestSideOfResized();
    }
}