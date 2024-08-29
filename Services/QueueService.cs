using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ABC_Retail.Services
{
    public class QueueService
    {
        private readonly QueueServiceClient _queueServiceClient;

        // Constructor: Initializes QueueServiceClient using connection string from configuration
        public QueueService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:QueueConnectionString"];
            _queueServiceClient = new QueueServiceClient(connectionString);
        }

        // Sends a message to a specified queue
        public async Task SendMessageAsync(string queueName, string message)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            await queueClient.CreateIfNotExistsAsync(); // Create the queue if it doesn't exist
            await queueClient.SendMessageAsync(message); // Send the message to the queue
        }
    }
}

