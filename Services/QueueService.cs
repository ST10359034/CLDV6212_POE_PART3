using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ABC_Retail.Services
{
    public class QueueService
    {
        private readonly QueueServiceClient _queueServiceClient;

        // Constructor initializes the QueueServiceClient using the provided connection string
        public QueueService(IConfiguration configuration)
        {
            _queueServiceClient = new QueueServiceClient(configuration["AzureStorage:QueueConnectionString"]);
        }

        // Method to send a message to the specified queue
        public async Task SendMessageAsync(string queueName, string message)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            await queueClient.CreateIfNotExistsAsync(); // Create the queue if it doesn't exist
            await queueClient.SendMessageAsync(message); // Send the message to the queue
        }
    }
}


