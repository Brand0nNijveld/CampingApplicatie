using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DataAccess
{
    public class DBConnection
    {
        private readonly string _connectionString;

        public DBConnection(string connectionString = "server=localhost;uid=root;pwd=;database=campingapplicatie;")
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

            _connectionString = connectionString;
        }

        // Returns a closed connection; the caller is responsible for opening it
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        // Returns a closed connection asynchronously; the caller is responsible for opening it
        public async Task<MySqlConnection> GetConnectionAsync()
        {
            var connection = new MySqlConnection(_connectionString);
            try
            {
                // Log connection creation for debugging purposes
                Debug.WriteLine("Creating a new database connection (async)...");
            }
            catch (Exception ex)
            {
                // Ensure the connection object is disposed if something goes wrong
                Debug.WriteLine($"Error creating database connection: {ex.Message}");
                connection.Dispose();
                throw;
            }

            return connection;
        }
    }
}
