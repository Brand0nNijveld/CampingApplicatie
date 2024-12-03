using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DataAccess
{
    public class DBconnection
    {
        private string _connectionString = "server=localhost;uid=root;pwd=;database=campingapplicatie;"; //als je username (uid), password of iets dergelijks anders is moet je dat ff aanpassen voor je eigen connectie;
        public MySqlConnection Connection { get; private set; }

        public DBconnection()
        {
            Connection = new MySqlConnection(_connectionString);
        }


    }
}
