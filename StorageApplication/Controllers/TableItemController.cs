using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageApplication.Model;
using StorageApplication.Interface;

namespace StorageApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableItemController : ControllerBase
    {
        private readonly ITableStorageRepository _storageService;
        public TableItemController(ITableStorageRepository storageService)
        {
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        [HttpGet("RetrieveById")]
        [ActionName(nameof(GetAsync))]
        public async Task<IActionResult> GetAsync([FromQuery] string category, string id)
        {
            var table = await _storageService.GetEntityAsync(category, id);
            if (table == null)
            {
                return NotFound();
            }
            return Ok(table);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> PostAsync([FromBody] TableEntity entity)
        {
            entity.PartitionKey = entity.Category;
            entity.RowKey = entity.Id;

            var createdEntity = await _storageService.AddEntityAsync(entity);
            return CreatedAtAction(nameof(GetAsync), createdEntity);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> PutAsync([FromBody] TableEntity entity)
        {
            entity.PartitionKey = entity.Category;
            entity.RowKey = entity.Id;

            await _storageService.UpsertEntityAsync(entity);
            return NoContent();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync([FromQuery] string category, string id)
        {
            await _storageService.DeleteEntityAsync(category, id);
            return NoContent();
        }
    }
}
