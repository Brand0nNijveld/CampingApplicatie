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
            _connectionString = connectionString;
        }

        
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

      
        public async Task<MySqlConnection> GetConnectionAsync()
        {
            var connection = new MySqlConnection(_connectionString);
            try
            {
                Debug.WriteLine("Opening database connection asynchronously...");
                await connection.OpenAsync();
                Debug.WriteLine("Database connection successfully opened.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error opening database connection: {ex.Message}");
                throw;  
            }

            return connection;
        }
    }
}
