namespace LodCore.Models
{
    public class Image
    {
        public Image(string bigPhotoName, string smallPhotoName)
        {
            BigPhotoName = bigPhotoName;
            SmallPhotoName = smallPhotoName;
        }

        public string BigPhotoName { get; private set; }
        public string SmallPhotoName { get; private set; }
    }
}