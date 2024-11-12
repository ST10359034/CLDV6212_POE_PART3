using Azure;
using Azure.Data.Tables;
using ABC_Retail.Models;
using System.Threading.Tasks;

namespace ABC_Retail.Services
{
    public class TableService
    {
        private readonly TableClient _tableClient;

        // Constructor initializes the TableClient using the provided connection string
        public TableService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:TableConnectionString"];
            var serviceClient = new TableServiceClient(connectionString);
            _tableClient = serviceClient.GetTableClient("CustomerProfiles");
            _tableClient.CreateIfNotExists(); // Create the table if it doesn't already exist
        }

        // Method to add a new customer profile entity to the table
        public async Task AddEntityAsync(CustomerProfile profile)
        {
            await _tableClient.AddEntityAsync(profile);
        }
    }
}
