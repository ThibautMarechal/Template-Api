﻿using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Api.Exceptions.StorageExceptions;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("/storage")]
    public class Storage : ControllerBase
    {
        private readonly IStorageService _storageService;

        public Storage(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<string> GetAllFiles()
        {
            return _storageService.GetFiles();
        }        
        
        [HttpGet("{file}")]
        public IActionResult GetFile([FromRoute(Name = "file")] string file)
        {
            try
            {
                return File(_storageService.GetFile(file), MediaTypeNames.Image.Jpeg);
            }
            catch (StorageFileNotFoundException)
            {
                return NotFound();
            }
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadFile(List<IFormFile> file)
        {
            if (file.Count != 1)
            {
                return UnprocessableEntity();
            }
            var fileId = await _storageService.CreateFile(file[0]).ConfigureAwait(false);
            return Accepted($"{Request.Path}/{fileId}");
        }        
        
        [HttpDelete("{file}")]
        [Authorize]
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