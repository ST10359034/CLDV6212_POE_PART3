using Azure;
using Azure.Data.Tables;
using System;

namespace ABC_Retail.Models
{
    // Product entity for Table Storage
    public class ProductInformation : ITableEntity
    {
        public string PartitionKey { get; set; }  // Group products
        public string RowKey { get; set; }        // Unique product ID
        public DateTimeOffset? Timestamp { get; set; }  // Last update time
        public ETag ETag { get; set; }            // Concurrency control

        public string ProductName { get; set; }   // Product name
        public string ProductDescription { get; set; } // Product description
        public decimal ProductPrice { get; set; } // Product price
        public string? ProductFilePath { get; set; } // File path (e.g., image)

        public ProductInformation()
        {
            PartitionKey = "ProductInformation";  // Default partition
            RowKey = Guid.NewGuid().ToString();   // Generate unique ID

            ProductName = string.Empty;
            ProductDescription = string.Empty;
            ProductPrice = 0.0m;
            ProductFilePath = string.Empty;
        }
    }
}
