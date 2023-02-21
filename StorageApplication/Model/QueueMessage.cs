namespace StorageApplication.Model
{
    public class QueueMessage
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public QueueMessage(string messageContent)
        {
            Message = messageContent;
        }
    }
}
