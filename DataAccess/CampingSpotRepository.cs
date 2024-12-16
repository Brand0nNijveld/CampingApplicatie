using CampingApplication.Business;
using CampingApplication.Business.CampingSpotService;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;

namespace DataAccess
{
    public class CampingSpotRepository : ICampingSpotRepository
    {
        private readonly DBConnection _dbConnection;

        public CampingSpotRepository(DBConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IEnumerable<CampingSpot> GetAvailableSpots(CampingSpot[] spots, DateTime startDate, DateTime endDate)
        {
            // Dummy implementation, returns an empty array
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
                using var connection = await _dbConnection.GetConnectionAsync();
                await connection.OpenAsync(); // Verantwoordelijkheid voor openen ligt bij de repository

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    availableSpots.Add(new CampingSpot(
                        reader.GetInt32("SpotNr"),
                        reader.GetInt32("PositionX"),
                        reader.GetInt32("PositionY")
                    ));
                }
                // Ensure the connection is closed after the operation
                connection.Close();  // Close the connection
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
                throw; // Rethrow exception for higher-level handling
            }

            return availableSpots;
        }

        public async Task<CampingSpotInfo> GetCampingSpotInfoAsync(int ID)
        {
            CampingSpotInfo campingSpotInfo = null;
            string query = "SELECT SpotNr, PricePerNight, Pets, Electricity FROM spotinfo WHERE SpotNr = @SpotNr";

            try
            {
                using var connection = await _dbConnection.GetConnectionAsync();
                await connection.OpenAsync(); // Verantwoordelijkheid voor openen ligt bij de repository

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
                // Ensure the connection is closed after the operation
                connection.Close();  // Close the connection
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
                throw; // Rethrow exception for higher-level handling
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
                using var connection = _dbConnection.GetConnection();
                connection.Open(); // Verantwoordelijkheid voor openen ligt bij de repository

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@SpotNr", ID);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    campingSpot = new CampingSpot(
                        reader.GetInt32("SpotNr"),
                        reader.GetInt32("PositionX"),
                        reader.GetInt32("PositionY")
                    );
                }
                // Ensure the connection is closed after the operation
                connection.Close();  // Close the connection
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
                throw; // Rethrow exception for higher-level handling
            }

            return campingSpot;
        }

        public IEnumerable<CampingSpot> GetCampingSpots()
        {
            var spots = new List<CampingSpot>();
            string query = "SELECT SpotNr, PositionX, PositionY FROM campingspot";

            try
            {
                using var connection = _dbConnection.GetConnection();
                connection.Open(); // Verantwoordelijkheid voor openen ligt bij de repository

                using var command = new MySqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    spots.Add(new CampingSpot(
                        reader.GetInt32("SpotNr"),
                        reader.GetInt32("PositionX"),
                        reader.GetInt32("PositionY")
                    ));
                }
                // Ensure the connection is closed after the operation
                connection.Close();  // Close the connection
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
                throw; // Rethrow exception for higher-level handling
            }

            return spots;
        }
    }
}

