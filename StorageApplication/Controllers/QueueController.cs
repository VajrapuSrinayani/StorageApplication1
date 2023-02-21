using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageApplication.Interface;
using StorageApplication.Model;

namespace StorageApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly IQueueRepository queueRepository;

        public QueueController(IQueueRepository queueRepository)
        {
            this.queueRepository = queueRepository;
        }

        //[HttpPost("CreateQueue")]
        //public async Task<IActionResult> CreateQueue(string qname)
        //{
        //    await queueRepository.CreateQueueAsync(qname);
        //    return Ok();
        //}

        [HttpPost("AddMessage")]
        public async Task<IActionResult> AddMessage(QueueMessage message)
        {
            await queueRepository.AddMessageAsync(message);
            return Ok();
        }

        [HttpGet("GetMessage")]
        public async Task<IActionResult> GetMessage()
        {
            var message = await queueRepository.GetMessageAsync();
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        [HttpGet]
        public async Task<QueueMessage> DequeueMessage()
        {
            return await queueRepository.DequeueMessageAsync();
        }

        [HttpPut]
        public async Task UpdateMessage(QueueMessage message)
        {
            await queueRepository.UpdateMessageAsync(message);
        }

        [HttpDelete("DeleteMessages")]
        public async Task<IActionResult> DeleteMessage()
        {
            await queueRepository.DeleteMessagesAsync();
            return Ok();
        }

        //[HttpDelete("DeleteQueue")]
        //public async Task<IActionResult> DeleteQueue()
        //{
        //    await queueRepository.DeleteQueueAsync();
        //    return Ok();
        //}
    }
}
