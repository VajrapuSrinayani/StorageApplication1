using Newtonsoft.Json;
using StorageApplication.Interface;
using StorageApplication.Model;
using Azure.Storage.Queues;

namespace StorageApplication.Repository
{
    public class QueueRepository: IQueueRepository
    {
        private readonly QueueClient queueClient;

        public QueueRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("StorageConnectionString");
            string queueName = configuration.GetValue<string>("QueueStorage:QueueName");
            queueClient = new QueueClient(connectionString, queueName);
        }

        //public async Task CreateQueueAsync(string qname)
        //{
        //    try
        //    {
        //        queueClient.CreateIfNotExists();
        //        await queueClient.CreateAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        public async Task AddMessageAsync(QueueMessage message)
        {
            try
            {
                var messageBody = JsonConvert.SerializeObject(message);
                await queueClient.CreateIfNotExistsAsync();
                await queueClient.SendMessageAsync(messageBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<QueueMessage> GetMessageAsync()
        {
            var response = await queueClient.PeekMessageAsync();
            if (response == null || response.Value == null || response.Value.MessageText == null)
            {
                return null;
            }
            var jsonMessage = response.Value.Body.ToString();
            var message = JsonConvert.DeserializeObject<QueueMessage>(jsonMessage);
            return message;
        }

        public async Task UpdateMessageAsync(QueueMessage message)
        {
            var receivedMessage = await queueClient.ReceiveMessageAsync();
            if (receivedMessage?.Value != null)
            {
                var updatedMessage = JsonConvert.DeserializeObject<QueueMessage>(receivedMessage.Value.MessageText);
                updatedMessage.Id = message.Id;
                updatedMessage.Message = message.Message;
                var messageBody = JsonConvert.SerializeObject(updatedMessage);
                await queueClient.UpdateMessageAsync(receivedMessage.Value.MessageId, receivedMessage.Value.PopReceipt, messageBody, TimeSpan.Zero);
            }
        }

        public async Task DeleteMessagesAsync()
        {
            await queueClient.ClearMessagesAsync();
        }

        //public async Task DeleteQueueAsync()
        //{  
        //    await queueClient.DeleteAsync();
        //}
    }
}
