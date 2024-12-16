using CampingApplication.Business;
using CampingApplication.Business.CampingSpotService;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;

namespace DataAccess
{
    public class CampingSpotRepository : ICampingSpotRepository
    {
        private readonly DBConnection _connection;

        public CampingSpotRepository(DBConnection con)
        {
            this._connection = con;
        }

        public IEnumerable<CampingSpot> GetAvailableSpots(CampingSpot[] spots, DateTime startDate, DateTime endDate)
        {
            return Array.Empty<CampingSpot>();
        }

        public async Task<IEnumerable<CampingSpot>> GetAvailableSpotsAsync(DateTime startDate, DateTime endDate)
        {
            var availableSpots = new List<CampingSpot>();
            string query = @"
                SELECT c.SpotNr, c.PositionX, c.PositionY
                FROM campingspot c
                JOIN camping ca ON c.CampingID = ca.CampingID
                WHERE c.SpotNr NOT IN (
                    SELECT b.SpotNr
                    FROM booking b
                    WHERE 
                        (StartDate >= @StartDate AND EndDate <= @EndDate) 
                        OR (EndDate >= @StartDate AND EndDate <= @EndDate)
                )";

            try
            {
                using var connection = await _connection.GetConnectionAsync();
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var id = reader.GetInt32("SpotNr");
                    var posX = reader.GetInt32("PositionX");
                    var posY = reader.GetInt32("PositionY");

                    availableSpots.Add(new CampingSpot(id, posX, posY));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
            }

            return availableSpots;
        }

        public async Task<CampingSpotInfo> GetCampingSpotInfoAsync(int ID)
        {
            CampingSpotInfo campingSpotInfo = null;
            string query = "SELECT SpotNr, PricePerNight, Pets, Electricity FROM spotinfo WHERE SpotNr = @SpotNr";

            try
            {
                using var connection = await _connection.GetConnectionAsync(); 
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@SpotNr", ID);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    campingSpotInfo = new CampingSpotInfo(
                        reader.GetInt32("SpotNr"),
                        reader.GetDouble("PricePerNight"),
                        reader.GetBoolean("Pets"),
                        reader.GetBoolean("Electricity")
                    );
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
            }

            if (campingSpotInfo == null)
            {
                Debug.WriteLine($"No camping info found for ID: {ID}");
            }

            return campingSpotInfo;
        }


        public CampingSpot GetCampingSpot(int ID)
        {
            CampingSpot campingSpot = null;
            string query = "SELECT SpotNr, PositionX, PositionY FROM campingspot WHERE SpotNr = @SpotNr";

            try
            {
                using var connection = _connection.GetConnection();
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@SpotNr", ID);

                connection.Open();

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    campingSpot = new CampingSpot(
                        reader.GetInt32("SpotNr"),
                        reader.GetInt32("PositionX"),
                        reader.GetInt32("PositionY")
                    );
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
            }

            return campingSpot;
        }

        public IEnumerable<CampingSpot> GetCampingSpots()
        {
            var spots = new List<CampingSpot>();
            string query = "SELECT SpotNr, PositionX, PositionY FROM campingspot";

            try
            {
                using var connection = _connection.GetConnection();
                using var command = new MySqlCommand(query, connection);

                connection.Open();

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var spot = new CampingSpot(
                        reader.GetInt32("SpotNr"),
                        reader.GetInt32("PositionX"),
                        reader.GetInt32("PositionY")
                    );

                    spots.Add(spot);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
            }

            return spots;
        }
    }
}
