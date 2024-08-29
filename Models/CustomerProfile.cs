using Azure;
using Azure.Data.Tables;
using System;

namespace ABC_Retail.Models
{
    public class CustomerProfile : ITableEntity
    {
        // Partition key for grouping entities
        public string PartitionKey { get; set; }

        // Unique key for identifying an entity within the partition
        public string RowKey { get; set; }

        // Timestamp of the entity's last modification
        public DateTimeOffset? Timestamp { get; set; }

        // ETag for concurrency control
        public ETag ETag { get; set; }

        // Custom properties for customer details
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Constructor: Sets default values for PartitionKey and generates a new RowKey
        public CustomerProfile()
        {
            PartitionKey = "CustomerProfile";
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
