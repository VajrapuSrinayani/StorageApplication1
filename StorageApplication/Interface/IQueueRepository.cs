﻿using StorageApplication.Model;

namespace StorageApplication.Interface
{
    public interface IQueueRepository
    {
        //Task CreateQueueAsync(string qname);
        Task AddMessageAsync(QueueMessage message);
        Task<QueueMessage> GetMessageAsync();
        Task<QueueMessage> DequeueMessageAsync();
        Task UpdateMessageAsync(QueueMessage message);
        Task DeleteMessagesAsync();

        //Task DeleteQueueAsync();
    }
}