using Azure;
using Azure.Data.Tables;
using System;

namespace ABC_Retail.Models
{
    // Order entity for Table Storage
    public class OrderInformation : ITableEntity
    {
        public string PartitionKey { get; set; }  // Group orders
        public string RowKey { get; set; }        // Unique order ID
        public DateTimeOffset? Timestamp { get; set; }  // Last update time
        public ETag ETag { get; set; }            // Concurrency control

        public int CustomerId { get; set; }       // Customer ID
        public int ProductId { get; set; }        // Product ID
        public DateTime OrderDate { get; set; }   // Order date
        public decimal TotalAmount { get; set; }  // Total order amount
        public string? OrderDetails { get; set; } // Additional details

        public OrderInformation()
        {
            PartitionKey = "OrderInformation";  // Default partition
            RowKey = Guid.NewGuid().ToString(); // Generate unique ID

            CustomerId = 0;
            ProductId = 0;
            OrderDate = DateTime.UtcNow;
            TotalAmount = 0.0m;
            OrderDetails = string.Empty;
        }
    }
}

