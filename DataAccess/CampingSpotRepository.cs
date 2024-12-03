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
                    SELECT c.Plaatsnummer, c.PositieX, c.PositieY
                    FROM campingplaats c
                    JOIN camping ca ON c.CampingID = ca.CampingID
                    WHERE c.Plaatsnummer NOT IN (
                        SELECT b.Plaatsnummer
                        FROM booking b
                        WHERE b.Begindatum < @Einddatum
                        AND b.Einddatum > @Begindatum
                    );";

                using (_connection.Connection)
                {
                    using (MySqlCommand command = new MySqlCommand(query, _connection.Connection))
                    {
                        command.Parameters.AddWithValue("@Begindatum", startDate);
                        command.Parameters.AddWithValue("@Einddatum", endDate);

                        _connection.Connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            int i = 0;
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("Plaatsnummer");
                                int posX = reader.GetInt32("PositieX");
                                int posY = reader.GetInt32("PositieY");

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
            catch (Exception ex) { Console.WriteLine($"Databasefout: {ex.Message}"); }

            return spots;
        }

        public CampingSpot GetCampingSpot(int ID)
        {
            try
            {
                string query = "SELECT Plaatsnummer, PositieX, PositieY FROM camping WHERE Plaatsnummer = @Plaatsnummer";

                using (_connection.Connection)
                {
                    using (MySqlCommand command = new MySqlCommand(query, _connection.Connection))
                    {
                        command.Parameters.AddWithValue("@Plaatsnummer", ID);

                        _connection.Connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int id = reader.GetInt32("Plaatsnummer");
                                int posX = reader.GetInt32("PositieX");
                                int posY = reader.GetInt32("PositieY");

                                CampingSpot result = new CampingSpot(id, posX, posY);

                                return result;
                            }
                        }
                        _connection.Connection.Close();
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Databasefout: {ex.Message}"); }

            return null;
        }

        public IEnumerable<CampingSpot> GetCampingSpots()
        {
            List<CampingSpot> Spots = new List<CampingSpot>();

            try
            {
                string query = "SELECT Plaatsnummer, PositieX, PositieY FROM Campingplaats";

                using (_connection.Connection)
                {
                    using (MySqlCommand command = new MySqlCommand(query, _connection.Connection))
                    {
                        _connection.Connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int id = reader.GetInt32("Plaatsnummer");
                                int posX = reader.GetInt32("PositieX");
                                int posY = reader.GetInt32("PositieY");

                                Spots.Add(new CampingSpot(id, posX, posY));
                            }
                        }
                        _connection.Connection.Close();
                        return Spots;
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Databasefout: {ex.Message}"); }

            return Spots;
        }
    }
}