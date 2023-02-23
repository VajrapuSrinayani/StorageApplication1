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
        public async Task<string> AddMessage(QueueMessage message)
        {
            var result = await queueRepository.AddMessageAsync(message);
            return result;
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

        [HttpGet("DequeueMessage")]
        public async Task<QueueMessage> DequeueMessage()
        {
            return await queueRepository.DequeueMessageAsync();
        }

        [HttpPut("UpdateMessage")]
        public async Task<string> UpdateMessage(QueueMessage message)
        {
            var result = await queueRepository.UpdateMessageAsync(message);
            return result;
        }

        [HttpDelete("DeleteMessages")]
        public async Task<string> DeleteMessage()
        {
            var result = await queueRepository.DeleteMessagesAsync();
            return result;
        }

        //[HttpDelete("DeleteQueue")]
        //public async Task<IActionResult> DeleteQueue()
        //{
        //    await queueRepository.DeleteQueueAsync();
        //    return Ok();
        //}
    }
}
