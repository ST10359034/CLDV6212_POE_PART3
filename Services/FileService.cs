using Azure.Storage.Files.Shares;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace ABC_Retail.Services
{
    public class FileService
    {
        private readonly ShareServiceClient _shareServiceClient;

        // Constructor initializes the ShareServiceClient with the provided connection string
        public FileService(IConfiguration configuration)
        {
            _shareServiceClient = new ShareServiceClient(configuration["AzureStorage:FileConnectionString"]);
        }

        // Method to upload a file to the specified Azure file share
        public async Task UploadFileAsync(string shareName, string fileName, Stream content)
        {
            var shareClient = _shareServiceClient.GetShareClient(shareName);
            await shareClient.CreateIfNotExistsAsync(); // Ensure the share exists

            var directoryClient = shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);
            await fileClient.CreateAsync(content.Length); // Create the file with the specified length
            await fileClient.UploadAsync(content); // Upload the file content
        }
    }
}
