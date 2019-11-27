using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Exceptions.StorageExceptions;
using Microsoft.AspNetCore.Http;

namespace Api.Services
{
    public class StorageService : IStorageService
    {
        private readonly StorageConfiguration _storageConfiguration; 

        public StorageService(StorageConfiguration storageConfiguration)
        {
            _storageConfiguration = storageConfiguration;
        }

        public IEnumerable<string> GetFiles() =>  Directory.GetFiles(_storageConfiguration.Path).Select(Path.GetFileName);

        public async Task<string> CreateFileAsync(IFormFile file)
        {
            var contentType = file.ContentType.Substring(file.ContentType.IndexOf("/", StringComparison.Ordinal) + 1);
            var fileId = $"{ Guid.NewGuid().ToString()}.{contentType}";
            try
            {
                await using var stream = File.Create(Path.Combine(_storageConfiguration.Path, fileId));
                await file.CopyToAsync(stream);
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

        public Stream GetFile(string fileId)
        {
            var filePath = Path.Combine(_storageConfiguration.Path, fileId);
            if (!File.Exists(Path.Combine(_storageConfiguration.Path, filePath)))
                throw new StorageFileNotFoundException(fileId);
            return File.OpenRead(filePath);
        }
    }
}