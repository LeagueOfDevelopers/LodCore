using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LodCore.Infrastructure.FilesManagement
{
    public class CustomMultipartStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartStreamProvider(string rootPath) : base(rootPath)
        {
        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            var fileName = headers?.ContentDisposition?.FileName?.Trim('"');
            fileName = fileName ?? base.GetLocalFileName(headers);
            return Path.GetFileName(fileName);
        }
    }
}