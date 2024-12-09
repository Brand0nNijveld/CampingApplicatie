using CampingApplication.Business;
using CampingApplication.Business.CampingSpotService;
using MySql.Data.MySqlClient;

namespace DataAccess
{
    public class CampingSpotRepository : ICampingSpotRepository
    {
        private DBconnection _connection;

        public CampingSpotRepository(DBconnection con)
        {
            this._connection = con;
        }

        public IEnumerable<CampingSpot> GetAvailableSpots(CampingSpot[] spots, DateTime startDate, DateTime endDate)
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
                        WHERE b.Startdate < @Enddate
                        AND b.Enddate > @Startdate
                    );";

                using (_connection.Connection)
                {
                    using (MySqlCommand command = new MySqlCommand(query, _connection.Connection))
                    {
                        command.Parameters.AddWithValue("@Startdate", startDate);
                        command.Parameters.AddWithValue("@Enddate", endDate);

                        _connection.Connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
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
                        _connection.Connection.Close();
                        Array.Resize(ref spots, availableSpots.Count);
                        for (int i = 0; i < availableSpots.Count; i++)
                        {
                            spots[i] = availableSpots[i];
                        }
                        return spots;
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Database error: {ex.Message}"); }

            return spots;
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

        public string AddCampingSpot(int ID, int X, int Y)
        {
            
            try
            {
                string query = "INSERT INTO campingspot (SpotNr, PositionX, PositionY) VALUES (@SpotNr, @PositionX, @PositionY)";

                using (_connection.Connection)
                {
                    using (MySqlCommand command = new MySqlCommand(query, _connection.Connection))
                    {
                        command.Parameters.AddWithValue("@SpotNr", ID);
                        command.Parameters.AddWithValue("@PositionX", X);
                        command.Parameters.AddWithValue("@PositionY", Y);

                        _connection.Connection.Open();

                        _connection.Connection.Close();
                    }
                }
                if (GetCampingSpot(ID) != null)
                {
                    return "Toevoegen gelukt!";   
                }
                else
                {
                    return "Toevoegen niet gelukt";
                }

            }
            catch (Exception ex) { return $"Database error: {ex.Message}"; }
        }

    }
}