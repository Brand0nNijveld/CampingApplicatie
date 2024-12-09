using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DataAccess
{
    public class DBConnection
    {
        private string _connectionString = "server=localhost;uid=root;pwd=;database=campingapplicatie;"; //als je username (uid), password of iets dergelijks anders is moet je dat ff aanpassen voor je eigen connectie;
        public MySqlConnection Connection { get; private set; }

        public DBConnection()
        {
            Connection = new MySqlConnection(_connectionString);
        }

        public async Task<MySqlConnection> GetConnectionAsync()
        {
            if (Connection.State != System.Data.ConnectionState.Open)
            {
                await Connection.OpenAsync();
            }

            return Connection;
        }
    }
}