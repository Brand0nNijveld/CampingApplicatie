using CampingApplication.Business;
using CampingApplication.Business.CampingSpotService;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

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

                connection.Close(); // Eigenlijk niet nodig aangezien 'using' dit al automatisch doet
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
                throw;
            }

            return availableSpots;
        }

        // Voeg een foto toe met het volledige pad en bestandsnaam
        public async Task AddFotoToDatabaseAsync(int spotNr, string filePath)
        {
            string fileName = Path.GetFileName(filePath);  // Haal de bestandsnaam uit het pad

            string query = @"
                INSERT INTO CampingSpotImage (SpotNr, FilePath, Filename)
                VALUES (@SpotNr, @FilePath, @Filename)";

            try
            {
                using var connection = await _dbConnection.GetConnectionAsync();
                await connection.OpenAsync();

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@SpotNr", spotNr);
                command.Parameters.AddWithValue("@FilePath", filePath);  // Sla het volledige pad op
                command.Parameters.AddWithValue("@Filename", fileName);    // Sla de bestandsnaam op

                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding photo: {ex.Message}");
            }
        }

        // Haal de camping info op, inclusief foto pad en bestandsnaam
        public async Task<CampingSpotInfo> GetCampingSpotInfoAsync(int ID)
        {
            CampingSpotInfo campingSpotInfo = null;
            string query = @"
                SELECT      
                spotinfo.SpotNr, spotinfo.PricePerNight, spotinfo.Pets, spotinfo.Electricity, 
                campingspot.Type, campingspot.Length, campingspot.Width, 
                CampingSpotImage.FilePath, CampingSpotImage.Filename
                FROM spotinfo 
                JOIN campingspot ON spotinfo.SpotNr = campingspot.SpotNr 
                LEFT JOIN CampingSpotImage ON spotinfo.SpotNr = CampingSpotImage.SpotNr 
                WHERE spotinfo.SpotNr = @SpotNr";

            try
            {
                using var connection = await _dbConnection.GetConnectionAsync();
                await connection.OpenAsync(); // Verantwoordelijkheid voor openen db-connectie ligt bij de repository

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@SpotNr", ID);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    campingSpotInfo = new CampingSpotInfo(
                        reader.GetInt32("SpotNr"),
                        reader.GetDouble("PricePerNight"),
                        reader.GetBoolean("Pets"),
                        reader.GetBoolean("Electricity"),
                        reader.GetDouble("Length"),
                        reader.GetDouble("Width"),
                        reader.GetString("Type")
                    );

                    // Optioneel, voeg de afbeelding gegevens toe aan campingSpotInfo
                    string filePath = reader.GetString("FilePath");
                    string fileName = reader.GetString("Filename");
                    // Gebruik de foto gegevens hier, bijvoorbeeld in je model of view
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
                throw;
            }

            if (campingSpotInfo == null)
            {
                Debug.WriteLine($"No camping info found for ID: {ID}");
            }

            return campingSpotInfo;
        }

        // Haal een camping spot op via SpotNr
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

                connection.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
                throw;
            }

            return campingSpot;
        }

        // Haal alle camping spots op
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

                connection.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
                throw;
            }

            return spots;
        }

        // Haal alle afbeeldingen voor een camping spot op
        public async Task<IEnumerable<CampingSpotImage>> GetCampingSpotImagesAsync(int spotNr)
        {
            var images = new List<CampingSpotImage>();
            string query = "SELECT PhotoID, FilePath, Filename FROM CampingSpotImage WHERE SpotNr = @SpotNr";

            try
            {
                using var connection = await _dbConnection.GetConnectionAsync();
                await connection.OpenAsync();

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@SpotNr", spotNr);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var image = new CampingSpotImage
                    {
                        PhotoID = reader.GetInt32("PhotoID"),
                        FilePath = reader.GetString("FilePath"),  // Haal het volledige pad op
                        Filename = reader.GetString("Filename")    // Haal de bestandsnaam op
                    };
                    images.Add(image);
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
            }

            return images;
        }
    }
}
