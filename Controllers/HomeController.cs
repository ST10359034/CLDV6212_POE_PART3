using Microsoft.AspNetCore.Mvc;
using ABC_Retail.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using ABC_Retail.Services; // Include the new services namespace
using Microsoft.AspNetCore.Http;
using System;

namespace ABC_Retail.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly CustomerService _customerService; // Inject CustomerService
        private readonly ProductService _productService; // Inject ProductService
        private readonly OrderService _orderService; // Inject OrderService
        private readonly BlobService _blobService; // Inject BlobService

        public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger, IConfiguration configuration, CustomerService customerService, ProductService productService, OrderService orderService, BlobService blobService)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
            _customerService = customerService;
            _productService = productService;
            _orderService = orderService;
            _blobService = blobService;
        }

        // Action for Index page, initialized with empty models for customer, product, and order
        public IActionResult Index()
        {
            var model = new { Customer = new CustomerProfile(), Product = new ProductInformation(), Order = new OrderInformation() };
            return View();
        }

        // Action to store customer information and insert it into SQL and Azure Table
        [HttpPost]
        public async Task<IActionResult> StoreCustomerInfo(CustomerProfile profile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Call Azure function to store data in Azure Table
                    using var httpClient = _httpClientFactory.CreateClient();
                    var baseUrl = _configuration["AzureFunctions:StoreTableInfo"];
                    var requestUri = $"{baseUrl}&tableName=CustomerProfiles&partitionKey={profile.PartitionKey}&rowKey={profile.RowKey}&firstName={profile.FirstName}&lastName={profile.LastName}&phoneNumber={profile.PhoneNumber}&Email={profile.Email}";

                    var response = await httpClient.PostAsync(requestUri, null);

                    if (response.IsSuccessStatusCode)
                    {
                        // Insert customer data into SQL database
                        await _customerService.InsertCustomerAsync(profile);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _logger.LogError($"Error submitting client info: {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception occurred while submitting client info: {ex.Message}");
                }
            }

            return View("Index", profile);
        }

        // Action to upload product image and insert data into SQL and Azure Blob Storage
        [HttpPost]
        public async Task<IActionResult> UploadProductImage(IFormFile imageFile)
        {
            if (imageFile != null)
            {
                try
                {
                    // Upload the blob to Azure Blob Storage
                    using var httpClient = _httpClientFactory.CreateClient();
                    using var stream = imageFile.OpenReadStream();
                    var content = new StreamContent(stream);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imageFile.ContentType);

                    var baseUrl = _configuration["AzureFunctions:UploadBlob"];
                    string url = $"{baseUrl}&blobName={imageFile.FileName}";
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Convert image to byte array for SQL insertion
                        using (var memoryStream = new MemoryStream())
                        {
                            await imageFile.CopyToAsync(memoryStream);
                            var imageData = memoryStream.ToArray();

                            // Insert image data into SQL BlobTable
                            await _blobService.InsertBlobAsync(imageData);
                        }

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _logger.LogError($"Error submitting image: {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception occurred while submitting image: {ex.Message}");
                }
            }
            else
            {
                _logger.LogError("No image file provided.");
            }

            return View("Index");
        }

        // Action to submit product information and store it in SQL and Azure Table
        [HttpPost]
        public async Task<IActionResult> StoreProductInfo(ProductInformation product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Call Azure function to store product data in Azure Table
                    using var httpClient = _httpClientFactory.CreateClient();
                    var baseUrl = _configuration["AzureFunctions:StoreTableInfo"];
                    var requestUri = $"{baseUrl}&tableName=ProductInformation&partitionKey={product.PartitionKey}&rowKey={product.RowKey}&productName={product.ProductName}&price={product.ProductPrice}";

                    var response = await httpClient.PostAsync(requestUri, null);

                    if (response.IsSuccessStatusCode)
                    {
                        // Insert product data into SQL database
                        await _productService.SaveProductInformationAsync(product);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _logger.LogError($"Error submitting product info: {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception occurred while submitting product info: {ex.Message}");
                }
            }

            return View("Index", product);
        }

        // Action to submit order information and store it in SQL and Azure Table
        [HttpPost]
        public async Task<IActionResult> StoreOrderInfo(OrderInformation order)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Call Azure function to store order data in Azure Table
                    using var httpClient = _httpClientFactory.CreateClient();
                    var baseUrl = _configuration["AzureFunctions:StoreTableInfo"];
                    var requestUri = $"{baseUrl}&tableName=OrderInformation&partitionKey={order.PartitionKey}&rowKey={order.RowKey}&customerId={order.CustomerId}&productId={order.ProductId}&orderDate={order.OrderDate}";

                    var response = await httpClient.PostAsync(requestUri, null);

                    if (response.IsSuccessStatusCode)
                    {
                        // Insert order data into SQL database
                        await _orderService.InsertOrderAsync(order);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _logger.LogError($"Error submitting order info: {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception occurred while submitting order info: {ex.Message}");
                }
            }

            return View("Index", order);
        }
    }
}



//____________________________________________________THIS IS PART 2 BELOW________________________________________________________________________________+
/*using Microsoft.AspNetCore.Mvc;
using SemesterTwo.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;*/
/*namespace SemesterTwo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var model = new CustomerProfile();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> StoreTableInfo(CustomerProfile profile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using var httpClient = _httpClientFactory.CreateClient();

                    var baseUrl = _configuration["AzureFunctions:StoreTableInfo"];
                    var requestUri = $"{baseUrl}&tableName=CustomerProfiles&partitionKey={profile.PartitionKey}&rowKey={profile.RowKey}&firstName={profile.FirstName}&lastName={profile.LastName}&phoneNumber={profile.PhoneNumber}&Email={profile.Email}";

                    var response = await httpClient.PostAsync(requestUri, null);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _logger.LogError($"Error submitting client info: {response.ReasonPhrase}");
                        _logger.LogError($"Response content: {await response.Content.ReadAsStringAsync()}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception occurred while submitting client info: {ex.Message}");
                }
            }

            return View("Index", profile);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile contractFile)
        {
            if (contractFile != null)
            {
                try
                {
                    using var httpClient = _httpClientFactory.CreateClient();
                    using var stream = contractFile.OpenReadStream();
                    var content = new StreamContent(stream);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contractFile.ContentType);

                    var baseUrl = _configuration["AzureFunctions:UploadFile"];
                    string url = $"{baseUrl}&fileName={contractFile.FileName}";
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _logger.LogError($"Error submitting document: {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception occurred while submitting document: {ex.Message}");
                }
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadBlob(IFormFile imageFile)
        {
            if (imageFile != null)
            {
                try
                {
                    using var httpClient = _httpClientFactory.CreateClient();
                    using var stream = imageFile.OpenReadStream();
                    var content = new StreamContent(stream);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imageFile.ContentType);

                    var baseUrl = _configuration["AzureFunctions:UploadBlob"];
                    string url = $"{baseUrl}&blobName={imageFile.FileName}";
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _logger.LogError($"Error submitting image: {response.ReasonPhrase}");
                        _logger.LogError($"Response content: {await response.Content.ReadAsStringAsync()}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception occurred while submitting image: {ex.Message}");
                }
            }
            else
            {
                _logger.LogError("No image file provided.");
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ProcessQueueMessage(string orderId)
        {
            if (!string.IsNullOrWhiteSpace(orderId))
            {
                try
                {
                    using var httpClient = _httpClientFactory.CreateClient();

                    var baseUrl = _configuration["AzureFunctions:ProcessQueueMessage"];
                    string url = $"{baseUrl}&message={orderId}";

                    var response = await httpClient.PostAsync(url, null);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _logger.LogError($"Error processing order: {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception occurred while processing order: {ex.Message}");
                }
            }
            else
            {
                _logger.LogError("No order ID provided.");
            }

            return RedirectToAction("Index");
        }
    }
}*/


/*using ABC_Retail.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ABC_Retail.Services;

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
            // Dependency injection for services
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

      
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                // Upload image to blob storage
                await _blobService.UploadBlobAsync("product-media", file.FileName, stream);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerProfile(CustomerProfile profile)
        {
            if (ModelState.IsValid)
            {
                // Add customer profile to the table
                await _tableService.AddEntityAsync(profile);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ProcessOrder(string orderId)
        {
            // Send order processing message to the queue
            await _queueService.SendMessageAsync("order-processing", $"Processing order {orderId}");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadContract(IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                // Upload contract file to file share
                await _fileService.UploadFileAsync("contracts-logs", file.FileName, stream);
            }
            return RedirectToAction("Index");
        }
    }
}
*/