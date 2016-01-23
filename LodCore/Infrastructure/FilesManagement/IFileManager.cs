using System.Net.Http;
using System.Threading.Tasks;

namespace FilesManagement
{
    public interface IFileManager
    {
        Task<string> UploadFileAsync(HttpContent content);

        Task<string> UploadImageAsync(HttpContent content);
    }
}