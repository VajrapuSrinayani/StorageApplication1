using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageApplication.Interface;
using StorageApplication.Model;

namespace StorageApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileRepository _fileRepository;
        public FileController(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        [HttpGet("RetrieveById")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var file = await _fileRepository.DownloadFile(fileName);

            if (file == null)
            {
                return NotFound();
            }

            return File(file, "application/octet-stream", fileName);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> UploadFile([FromForm] FileModel file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _fileRepository.UploadFile(file.FileDetail);

            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            var result = await _fileRepository.DeleteFile(fileName);

            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
