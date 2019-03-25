using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using LodCoreLibraryOld.Common;

namespace LodCoreLibraryOld.Infrastructure.FilesManagement
{
    public interface IFileManager
    {
        Stream GetFile(string fileName);

        Stream GetImage(string imageName);

        Task<string> UploadFileAsync(HttpContent content);

        Task<Image> UploadImageAsync(HttpContent content);
    }
}