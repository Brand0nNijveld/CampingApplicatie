using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.LoginService
{
    public class LoginRepositoryMock : ILoginRepository
    {
        private readonly List<(string Username, string Password)> mockUsers = new List<(string Username, string Password)>
        {
            ("testuser", "Password123"),
            ("admin", "Admin12345"),
            ("employee", "Employee2023")
        };

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            await Task.Delay(500);

            return mockUsers.Any(user => user.Username == username && user.Password == password);
        }
    }
}
