using System.Threading.Tasks;
using ABC_Retail.Models;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace ABC_Retail.Services
{
    public class ProductService
    {
        private readonly string _connectionString;
        private readonly ILogger<ProductService> _logger;

        // Constructor to initialize connection string and logger
        public ProductService(IConfiguration configuration, ILogger<ProductService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection"); // Retrieve connection string
            _logger = logger;
        }

        // Method to save product information in the database
        public async Task<bool> SaveProductInformationAsync(ProductInformation productInfo)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(); // Open database connection

                    // SQL query to insert product data
                    using (var command = new SqlCommand("INSERT INTO Products (ProductId, ProductName, ProductDescription, ProductPrice, ProductFile) VALUES (@ProductId, @ProductName, @ProductDescription, @ProductPrice, @ProductFile)", connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.Add(new SqlParameter("@ProductName", SqlDbType.NVarChar, 255) { Value = productInfo.ProductName });
                        command.Parameters.Add(new SqlParameter("@ProductDescription", SqlDbType.NVarChar, 1000) { Value = productInfo.ProductDescription });
                        command.Parameters.Add(new SqlParameter("@ProductPrice", SqlDbType.Decimal) { Value = productInfo.ProductPrice });
                        command.Parameters.Add(new SqlParameter("@ProductFile", SqlDbType.VarBinary) { Value = productInfo.ProductFilePath });

                        // Execute the query and check if the product was inserted successfully
                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0; // Return true if insertion was successful
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while saving product information: {ex.Message}"); // Log any error
                return false; // Return false if an error occurs
            }
        }
    }
}
