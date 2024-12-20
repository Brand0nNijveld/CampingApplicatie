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

        // geeft een gesloten connectie en de oproeper is verantwoordelijk voor het openen
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        // geeft een gesloten connectie 'asynchronisch' de oproeper is weer verantwoordelijk voor het openen
        public async Task<MySqlConnection> GetConnectionAsync()
        {
            var connection = new MySqlConnection(_connectionString);
            try
            {
                Debug.WriteLine("Creating a new database connection (async)...");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating database connection: {ex.Message}");
                connection.Dispose();
                throw;
            }

            return connection;
        }
    }
}
