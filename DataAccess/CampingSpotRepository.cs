using CampingApplication.Business;
using CampingApplication.Business.CampingSpotService;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataAccess
{
    public class CampingSpotRepository : ICampingSpotRepository
    {
        private DBConnection connection;

        public CampingSpotRepository(DBConnection con)
        {
            this.connection = con;
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
                        WHERE (b.StartDate >= @StartDate AND b.EndDate <= @EndDate)
                        OR (b.EndDate >= @StartDate AND b.StartDate <= @EndDate)
                    );";

                using (MySqlCommand command = new MySqlCommand(query, await connection.GetConnectionAsync()))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));
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
            catch (Exception ex)
            {
                Console.WriteLine($"Database error getting available spots: {ex.Message}");
                throw;
            }

            return availableSpots;
        }

        public CampingSpot GetCampingSpot(int ID)
        {
            try
            {
                string query = "SELECT SpotNr, PositionX, PositionY FROM camping WHERE SpotNr = @SpotNr";

                using (connection.Connection)
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection.Connection))
                    {
                        command.Parameters.AddWithValue("@SpotNr", ID);
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
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error getting camping spot: {ex.Message}");
                throw;
            }

            return null;
        }

        /// <summary>
        /// Synchronous database access is not supported. Use GetCampingSpotsAsync instead.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<CampingSpot> GetCampingSpots()
        {
            throw new NotImplementedException("Calling a database method synchronously");
        }

        public async Task<IEnumerable<CampingSpot>> GetCampingSpotsAsync()
        {
            List<CampingSpot> Spots = [];
            try
            {
                string query = "SELECT SpotNr, PositionX, PositionY FROM campingspot";

                using (var conn = await connection.GetConnectionAsync())
                {
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("SpotNr");
                                int posX = reader.GetInt32("PositionX");
                                int posY = reader.GetInt32("PositionY");

                                Spots.Add(new CampingSpot(id, posX, posY));
                            }
                        }
                        return Spots;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error getting camping spots: {ex.Message}");
                throw;
            }
        }
    }
}