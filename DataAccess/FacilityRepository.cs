using CampingApplication.Business;
using CampingApplication.Business.FacilityService;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class FacilityRepository : IFacilityRepository
    {
        private DBConnection connection;

        public FacilityRepository(DBConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IEnumerable<Facility>> GetFacilitiesAsync()
        {
            List<Facility> facilities = [];
            string query = "SELECT ID, Type, PositionX, PositionY FROM facility;";

            using (var conn = await connection.GetConnectionAsync())
            {
                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("ID");
                            string type = reader.GetString("Type");
                            double posX = reader.GetDouble("PositionX");
                            double posY = reader.GetDouble("PositionY");

                            Facility facility = new(id, posX, posY, type);
                            facilities.Add(facility);
                        }
                    }
                }
            }

            return facilities;
        }
    }
}
