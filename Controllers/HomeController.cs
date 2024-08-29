using ABC_Retail.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ABC_Retail.Services;
using Azure.Storage.Blobs.Models; // For BlobItem
using System.IO; // For Stream

namespace ABC_Retail.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlobService _blobService;
        private readonly TableService _tableService;
        private readonly QueueService _queueService;
        private readonly FileService _fileService;

        public HomeController(BlobService blobService, TableService tableService, QueueService queueService, FileService fileService)
        {
            _blobService = blobService;
            _tableService = tableService;
            _queueService = queueService;
            _fileService = fileService;
        }

        // Renders the Index page
        public IActionResult Index()
        {
            return View();
        }

        // Renders the Privacy page
        public IActionResult Privacy()
        {
            return View();
        }

        // Uploads an image to Azure Blob Storage
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                await _blobService.UploadBlobAsync("product-media", file.FileName, stream);
            }
            return RedirectToAction("Index");
        }

        // Adds a customer profile to Azure Table Storage
        [HttpPost]
        public async Task<IActionResult> AddCustomerProfile(CustomerProfile profile)
        {
            if (ModelState.IsValid)
            {
                await _tableService.AddEntityAsync(profile);
            }
            return RedirectToAction("Index");
        }

        // Sends a message to Azure Queue Storage to process an order
        [HttpPost]
        public async Task<IActionResult> ProcessOrder(string orderId)
        {
            await _queueService.SendMessageAsync("order-processing", $"Processing order {orderId}");
            return RedirectToAction("Index");
        }

        // Uploads a contract file to Azure File Storage
        [HttpPost]
        public async Task<IActionResult> UploadContract(IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                await _fileService.UploadFileAsync("contracts-logs", file.FileName, stream);
            }
            return RedirectToAction("Index");
        }

        // Lists all blobs in the "product-media" container
        [HttpPost]
        public async Task<IActionResult> ListBlobs()
        {
            var blobs = await _blobService.ListBlobsAsync("product-media");
            ViewBag.Blobs = blobs;
            return View("Index"); // Shows blobs on the Index page
        }

        // Downloads a specific blob from Azure Blob Storage
        public async Task<IActionResult> DownloadBlob(string blobName)
        {
            var blobStream = await _blobService.DownloadBlobAsync("product-media", blobName);
            return File(blobStream, "application/octet-stream", blobName); // Downloads the blob
        }
    }
}

