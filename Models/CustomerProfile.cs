using Azure;
using Azure.Data.Tables;
using System;

namespace ABC_Retail.Models
{
    // Customer profile entity for Table Storage
    public class CustomerProfile : ITableEntity
    {
        public string PartitionKey { get; set; }  // Group customer profiles
        public string RowKey { get; set; }        // Unique customer ID
        public DateTimeOffset? Timestamp { get; set; }  // Last update time
        public ETag ETag { get; set; }            // Concurrency control

        public string FirstName { get; set; }     // Customer's first name
        public string LastName { get; set; }      // Customer's last name
        public string Email { get; set; }         // Customer's email address
        public string PhoneNumber { get; set; }   // Customer's phone number
        public string? ContractFilePath { get; set; }  // Path to contract file (optional)
        public string? ImageFilePath { get; set; }     // Path to profile image (optional)

        public CustomerProfile()
        {
            PartitionKey = "CustomerProfile";  // Default partition
            RowKey = Guid.NewGuid().ToString(); // Generate unique ID

            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;

            ContractFilePath = string.Empty;
            ImageFilePath = string.Empty;
        }
    }
}


