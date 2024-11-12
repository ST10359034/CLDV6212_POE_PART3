using System;
using System.Threading.Tasks;
using ABC_Retail.Models;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace ABC_Retail.Services
{
    public class OrderService
    {
        private readonly string _connectionString;
        private readonly ILogger<OrderService> _logger;

        // Constructor to initialize connection string and logger
        public OrderService(IConfiguration configuration, ILogger<OrderService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection"); // Retrieve connection string
            _logger = logger;
        }

        // Method to insert order information into the database
        public async Task<bool> InsertOrderAsync(OrderInformation orderInfo)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(); // Open the connection

                    // SQL query for inserting order
                    string query = @"
                        INSERT INTO Orders (OrderId, CustomerId, ProductId, OrderDate, TotalAmount)
                        VALUES (@OrderId, @CustomerId, @ProductId, @OrderDate, @TotalAmount)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.Add(new SqlParameter("@CustomerId", SqlDbType.Int) { Value = orderInfo.CustomerId });
                        command.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.Int) { Value = orderInfo.ProductId });
                        command.Parameters.Add(new SqlParameter("@OrderDate", SqlDbType.DateTime) { Value = orderInfo.OrderDate });
                        command.Parameters.Add(new SqlParameter("@TotalAmount", SqlDbType.Decimal) { Value = orderInfo.TotalAmount });

                        // Execute query and return success status
                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while inserting order information: {ex.Message}"); // Log error
                return false; // Return false in case of an error
            }
        }
    }
}
