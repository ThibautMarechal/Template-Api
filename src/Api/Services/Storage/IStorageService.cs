using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Api.Services.Storage
{
    public interface IStorageService
    {
        IEnumerable<string> GetFiles();
        Task<string> CreateFileAsync(IFormFile file);
        void DeleteFile(string file);
        Stream GetFile(string file);
    }
}