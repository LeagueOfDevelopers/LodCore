using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Common;

namespace FilesManagement
{
    public interface IFileManager
    {
        Stream GetFile(string fileName);

        Stream GetImage(string imageName);

        Task<string> UploadFileAsync(HttpContent content);

        Task<Image> UploadImageAsync(HttpContent content);
    }
}