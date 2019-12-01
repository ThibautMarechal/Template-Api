using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Exceptions.StorageExceptions;
using Microsoft.AspNetCore.Http;

namespace Api.Services.Storage
{
    public class StorageService : IStorageService
    {
        private const string ContentTypeSuffix = "-content-type";
        private readonly StorageConfiguration _storageConfiguration; 

        public StorageService(StorageConfiguration storageConfiguration)
        {
            _storageConfiguration = storageConfiguration;
        }

        public IEnumerable<string> GetFiles() =>  Directory.GetFiles(_storageConfiguration.Path).Select(Path.GetFileName);

        public async Task<string> CreateFileAsync(IFormFile file)
        {
            var fileId = Guid.NewGuid().ToString();
            try
            {
                var filePath = Path.Combine(_storageConfiguration.Path, fileId);
                await using var stream = File.Create(filePath);
                await file.CopyToAsync(stream);
                await using var contentTypeStream =
                    File.Create($"{filePath}{ContentTypeSuffix}");
                await contentTypeStream.WriteAsync(new UnicodeEncoding().GetBytes(file.ContentType));
            }
            catch (Exception e)
            {
                throw new StorageFileCreateException(fileId, e);
            }
            return fileId;
        }

        public void DeleteFile(string fileId)
        {
            var filePath = Path.Combine(_storageConfiguration.Path, fileId);
            if (!File.Exists(filePath))
                throw new StorageFileNotFoundException(fileId);
            try
            {
                File.Delete(filePath);
            }
            catch (Exception e)
            {
                throw new StorageFileDeleteException(fileId, e);
            }
        }

        public Stream GetFile(string fileId, out string contentType)
        {
            var filePath = Path.Combine(_storageConfiguration.Path, fileId);
            var fileContentTypePath = $"{filePath}{ContentTypeSuffix}";
            if (!File.Exists(filePath) || !File.Exists(fileContentTypePath))
                throw new StorageFileNotFoundException(fileId);
            contentType = File.ReadAllText(fileContentTypePath, Encoding.Unicode);
            return File.OpenRead(filePath);
        }
    }
}