using CampingApplication.Business;
using CampingApplication.Business.BookingService;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Bookings
{
    public class BookingRepository : IBookingRepository
    {
        private DBConnection connection;

        public BookingRepository(DBConnection con)
        {
            connection = con;
        }

        public async Task<IEnumerable<Booking>> GetBookingsInTimeFrameAsync(int campingSpotID, DateTime startDate, DateTime endDate)
        {
            List<Booking> bookings = new List<Booking>();

            string query = @"SELECT BookingID, SpotNr, StartDate, EndDate
                     FROM booking
                     WHERE SpotNr = @SpotID
                     AND (
                         (StartDate < @EndDate AND EndDate > @StartDate)
                     )";

            using (var cmd = new MySqlCommand(query, await connection.GetConnectionAsync()))
            {
                cmd.Parameters.AddWithValue("@SpotID", campingSpotID);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int ID = reader.GetInt32("BookingID");
                        int spotNr = reader.GetInt32("SpotNr");
                        DateTime sDate = reader.GetDateTime("StartDate");
                        DateTime eDate = reader.GetDateTime("EndDate");

                        bookings.Add(new Booking(ID, sDate, eDate));
                    }
                }
            }

            return bookings;
        }

        public async Task SaveBookingAsync(BookingRequest request)
        {
            MySqlConnection conn = await connection.GetConnectionAsync();
            using (var transaction = await conn.BeginTransactionAsync())
            {
                try
                {
                    // Check if there are any bookings overlapping
                    var bookings = await GetBookingsInTimeFrameAsync(request.CampingSpotID, request.StartDate, request.EndDate);
                    Debug.WriteLine("Bookings found: " + bookings.Count());
                    if (bookings.Count() > 0)
                    {
                        throw new BookingException("You are trying to book for a period that already has bookings.", BookingExceptionType.AlreadyBooked);
                    }

                    string insertBookingQuery = "INSERT INTO booking (SpotNr, StartDate, EndDate, Pets, Electricity) VALUES (@CampingID, @StartDate, @EndDate, @Pets, @Electricity)";
                    using (var cmd = new MySqlCommand(insertBookingQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CampingID", request.CampingSpotID);
                        cmd.Parameters.AddWithValue("@StartDate", request.StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", request.EndDate);
                        cmd.Parameters.AddWithValue("@Pets", request.Pets);
                        cmd.Parameters.AddWithValue("@Electricity", request.Electricity);
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
                    Debug.WriteLine("Error saving booking to database: " + ex.Message);
                    throw;
                }
            }
        }
    }
}
