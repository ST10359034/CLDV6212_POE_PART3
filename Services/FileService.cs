using Azure.Storage.Files.Shares;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace ABC_Retail.Services
{
    public class FileService
    {
        private readonly ShareServiceClient _shareServiceClient;

        // Constructor: Initializes ShareServiceClient using connection string from configuration
        public FileService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:FileConnectionString"];
            _shareServiceClient = new ShareServiceClient(connectionString);
        }

        // Uploads a file to a specified share
        public async Task UploadFileAsync(string shareName, string fileName, Stream content)
        {
            var shareClient = _shareServiceClient.GetShareClient(shareName);
            await shareClient.CreateIfNotExistsAsync(); // Create the share if it doesn't exist
            var directoryClient = shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);
            await fileClient.CreateAsync(content.Length); // Create the file with the specified length
            await fileClient.UploadAsync(content); // Upload the file content
        }
    }
}
