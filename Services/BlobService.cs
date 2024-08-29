using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ABC_Retail.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        // Constructor: Initializes BlobServiceClient using connection string from configuration
        public BlobService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:BlobConnectionString"];
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        // Uploads a blob to a specified container
        public async Task UploadBlobAsync(string containerName, string blobName, Stream content)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(); // Create container if it doesn't exist
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(content, true); // Upload the blob with overwrite option
        }

        // Downloads a blob from a specified container
        public async Task<Stream> DownloadBlobAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            var blobDownloadInfo = await blobClient.DownloadAsync(); // Download the blob

            return blobDownloadInfo.Value.Content; // Return the blob content as a stream
        }

        // Lists all blobs in a specified container
        public async Task<IEnumerable<BlobItem>> ListBlobsAsync(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = new List<BlobItem>();

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                blobs.Add(blobItem); // Add each blob item to the list
            }

            return blobs; // Return the list of blob items
        }
    }
}

