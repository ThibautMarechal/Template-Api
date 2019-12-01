using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Api.Exceptions.StorageExceptions;
using Api.Services;
using Api.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("/storage")]
    [Authorize]
    public class Storage : ControllerBase
    {
        private readonly IStorageService _storageService;

        public Storage(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpGet]
        public IEnumerable<string> GetAllFiles()
        {
            return _storageService.GetFiles();
        }        
        
        [AllowAnonymous]
        [HttpGet("{file}")]
        public IActionResult GetFile([FromRoute(Name = "file")] string file)
        {
            try
            {
                var stream = _storageService.GetFile(file, out var contentType);
                return File(stream, contentType);
            }
            catch (StorageFileNotFoundException)
            {
                return NotFound();
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> UploadFileAsync(List<IFormFile> file)
        {
            if (file.Count != 1)
                return UnprocessableEntity();
            
            var fileId = await _storageService.CreateFileAsync(file[0]).ConfigureAwait(false);
            return Accepted($"{Request.Path}/{fileId}");
        }        
        
        [HttpDelete("{file}")]
        public IActionResult DeleteFile([FromRoute(Name = "file")] string file)
        {
            try
            {
                _storageService.DeleteFile(file);
                return NoContent();
            }
            catch (StorageFileNotFoundException)
            {
                return NotFound();
            }
            catch (StorageFileDeleteException)
            {
                return StatusCode(500);
            }
        }
    }
}