using Azure.Storage.Queues;

namespace ProducerWebJob.Configuration
{
    public class Settings
    {
        public string AzureQueueName { get; set; }

        public string AzureWebJobsStorage { get; set; }

        public QueueClientOptions QueueClientOptions { get; set; } = new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        };
    }
}
