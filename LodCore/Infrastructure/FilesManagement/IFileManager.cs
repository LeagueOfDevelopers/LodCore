using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace FilesManagement
{
    public interface IFileManager
    {
        Stream GetFile(string fileName);

        Task<string> UploadFileAsync(HttpContent content);

        Task<string> UploadImageAsync(HttpContent content);
    }
}