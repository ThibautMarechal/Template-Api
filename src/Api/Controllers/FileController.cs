using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Api.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Api.Controllers
{
    [ApiController]
    [Route("/file")]
    public class FileController : ControllerBase
    {
        private readonly IFileProvider _fileProvider;
        private readonly FileConfiguration _fileConfiguration;

        public FileController(IFileProvider fileProvider, FileConfiguration fileConfiguration)
        {
            _fileProvider = fileProvider;
            _fileConfiguration = fileConfiguration;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<string> GetAllFiles()
        {
            return _fileProvider.GetDirectoryContents(string.Empty).Select(c => c.Name);
        }        
        
        [HttpGet("{file}")]
        public IActionResult GetFile([FromRoute(Name = "file")] string file)
        {
            if (_fileProvider.GetFileInfo(file).Exists)
            {
                var fileFullPath = Path.Combine(_fileConfiguration.Path, file);
                return File(System.IO.File.OpenRead(fileFullPath), MediaTypeNames.Image.Jpeg);
            }
            return NotFound();
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadFile(byte[] file)
        {
            if (file.Length > _fileConfiguration.MaxSize)
            {
                return BadRequest();
            }
            var fileFullPath = Path.Combine(_fileConfiguration.Path, Guid.NewGuid().ToString());
            await using var stream = System.IO.File.Create(fileFullPath);
            await stream.WriteAsync(file).ConfigureAwait(false);
            return Created(new Uri($"{Request.Path}/{fileFullPath}"), null);
        }        
        
        [HttpDelete("{file}")]
        [Authorize]
        public IActionResult DeleteFile([FromRoute(Name = "file")] string file)
        {
            if (!_fileProvider.GetFileInfo(file).Exists) return NotFound();
            var filePath = Path.Combine(_fileConfiguration.Path, file);
            System.IO.File.Delete(filePath);
            return NoContent();
        }
    }
}