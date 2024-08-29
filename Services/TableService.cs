using Azure;
using Azure.Data.Tables;
using ABC_Retail.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ABC_Retail.Services
{
    public class TableService
    {
        private readonly TableClient _tableClient;

        // Constructor: Initializes TableClient with connection string from configuration
        public TableService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:TableConnectionString"];
            var serviceClient = new TableServiceClient(connectionString);
            _tableClient = serviceClient.GetTableClient("CustomerProfiles");
            _tableClient.CreateIfNotExists(); // Ensure the table exists
        }

        // Adds a customer profile entity to the "CustomerProfiles" table
        public async Task AddEntityAsync(CustomerProfile profile)
        {
            await _tableClient.AddEntityAsync(profile); // Add the entity to the table
        }
    }
}
