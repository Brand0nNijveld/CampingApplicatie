using CampingApplication.Business.LoginService;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.Common;
using System.Data;

namespace DataAccess
{
    public class LoginRepository : ILoginRepository
    {
        private readonly DBConnection connection;

        // Constructor accepting a DBConnection object
        public LoginRepository(DBConnection con)
        {
            connection = con;
        }

        // Implement the ValidateUserAsync method from ILoginRepository
        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            // Set up the connection to the database
            MySqlConnection conn = await connection.GetConnectionAsync();

            // Query to validate the user based on username and password
            string query = @"SELECT COUNT(1) 
                             FROM users 
                             WHERE Username = @Username 
                             AND Password = @Password";

            try
            {
                // Create and execute the command to query the database
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password); // Consider hashing passwords in production

                    // Execute the query and check if a valid user exists
                    var result = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    return result > 0;  // Return true if user exists, false otherwise
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions
                Debug.WriteLine("Error validating user: " + ex.Message);
                return false;  // Return false if an exception occurs during validation
            }
        }
    }
}
