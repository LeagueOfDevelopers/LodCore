namespace LodCoreApiOld.Models
{
    public class Image
    {
        public Image(string bigPhotoName, string smallPhotoName)
        {
            BigPhotoName = bigPhotoName;
            SmallPhotoName = smallPhotoName;
        }

        public string BigPhotoName { get; }
        public string SmallPhotoName { get; }
    }
}