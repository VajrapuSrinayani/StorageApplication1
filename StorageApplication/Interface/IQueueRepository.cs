using StorageApplication.Model;

namespace StorageApplication.Interface
{
    public interface IQueueRepository
    {
        //Task CreateQueueAsync(string qname);
        Task<string> AddMessageAsync(QueueMessage message);
        Task<QueueMessage> GetMessageAsync();
        Task<QueueMessage> DequeueMessageAsync();
        Task<string> UpdateMessageAsync(QueueMessage message);
        Task<string> DeleteMessagesAsync();

        //Task DeleteQueueAsync();
    }
}
