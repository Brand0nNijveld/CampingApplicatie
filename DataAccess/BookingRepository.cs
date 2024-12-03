using CampingApplication.Business.BookingService;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BookingRepository : IBookingRepository
    {
        private DBconnection connection;

        public BookingRepository(DBconnection con)
        {
            connection = con;
        }

        public async Task SaveBookingAsync(BookingRequest request)
        {
            MySqlConnection conn = await connection.GetConnectionAsync();
            using (var transaction = await conn.BeginTransactionAsync())
            {
                try
                {
                    string insertBookingQuery = "INSERT INTO booking (SpotNr, StartDate, EndDate) VALUES (@CampingID, @StartDate, @EndDate)";
                    using (var cmd = new MySqlCommand(insertBookingQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CampingID", request.CampingSpotID);
                        cmd.Parameters.AddWithValue("@StartDate", request.StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", request.EndDate);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    string getLastInsertIdQuery = "SELECT LAST_INSERT_ID();";
                    int bookingID;
                    using (var cmd = new MySqlCommand(getLastInsertIdQuery, conn, transaction))
                    {
                        bookingID = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    }

                    string insertCustomerInfoQuery =
                        @"INSERT INTO customerinfo (BookingID, Name, Email, TelNr)
                        VALUES(@BookingID, @Name, @Email, @TelNr)";
                    using (var cmd = new MySqlCommand(insertCustomerInfoQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@BookingID", bookingID);
                        cmd.Parameters.AddWithValue("@Name", request.FirstName + " " + request.LastName);
                        cmd.Parameters.AddWithValue("@Email", request.Email);
                        cmd.Parameters.AddWithValue("@TelNr", request.PhoneNumber);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Debug.WriteLine(ex.Message);
                    throw new Exception("Pope");
                }
            }
        }
    }
}
