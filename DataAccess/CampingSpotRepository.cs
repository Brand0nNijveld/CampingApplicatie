using CampingApplication.Business;
using CampingApplication.Business.CampingSpotService;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataAccess
{
    public class CampingSpotRepository : ICampingSpotRepository
    {
        private DBConnection _connection;

        public CampingSpotRepository(DBConnection con)
        {
            this._connection = con;
        }

        public IEnumerable<CampingSpot> GetAvailableSpots(CampingSpot[] spots, DateTime startDate, DateTime endDate)
        {
            return [];
        }

        public async Task<IEnumerable<CampingSpot>> GetAvailableSpotsAsync(DateTime startDate, DateTime endDate)
        {
            List<CampingSpot> availableSpots = new List<CampingSpot>();

            try
            {
                string query = @"
                    SELECT c.SpotNr, c.PositionX, c.PositionY
                    FROM campingspot c
                    JOIN camping ca ON c.CampingID = ca.CampingID
                    WHERE c.SpotNr NOT IN (
                        SELECT b.SpotNr
                        FROM booking b
                        AND ((StartDate >= @StartDate AND EndDate <= @EndDate) 
                        OR (EndDate >= @StartDate && EndDate <= @EndDate))";

                using (_connection.Connection)
                {
                    using (MySqlCommand command = new MySqlCommand(query, _connection.Connection))
                    {
                        command.Parameters.AddWithValue("@Startdate", startDate);
                        command.Parameters.AddWithValue("@Enddate", endDate);

                        _connection.Connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            int i = 0;
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("SpotNr");
                                int posX = reader.GetInt32("PositionX");
                                int posY = reader.GetInt32("PositionY");

                                availableSpots.Add(new CampingSpot(id, posX, posY));
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Database error: {ex.Message}"); }

            return availableSpots;
        }

        public CampingSpot GetCampingSpot(int ID)
        {
            try
            {
                string query = "SELECT SpotNr, PositionX, PositionY FROM camping WHERE SpotNr = @SpotNr";

                using (_connection.Connection)
                {
                    using (MySqlCommand command = new MySqlCommand(query, _connection.Connection))
                    {
                        command.Parameters.AddWithValue("@SpotNr", ID);

                        _connection.Connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int id = reader.GetInt32("SpotNr");
                                int posX = reader.GetInt32("PositionX");
                                int posY = reader.GetInt32("PositionY");

                                CampingSpot result = new CampingSpot(id, posX, posY);

                                return result;
                            }
                        }
                        _connection.Connection.Close();
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Database error: {ex.Message}"); }

            return null;
        }

        public Task<CampingSpotInfo> GetCampingSpotInfoAsync(int ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CampingSpot> GetCampingSpots()
        {
            List<CampingSpot> Spots = new List<CampingSpot>();

            try
            {
                string query = "SELECT SpotNr, PositionX, PositionY FROM campingspot";

                using (_connection.Connection)
                {
                    using (MySqlCommand command = new MySqlCommand(query, _connection.Connection))
                    {
                        _connection.Connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int id = reader.GetInt32("SpotNr");
                                int posX = reader.GetInt32("PositionX");
                                int posY = reader.GetInt32("PositionY");

                                Spots.Add(new CampingSpot(id, posX, posY));
                            }
                        }
                        _connection.Connection.Close();
                        return Spots;
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Database error: {ex.Message}"); }

            return Spots;
        }
    }
}