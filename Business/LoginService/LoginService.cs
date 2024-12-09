using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CampingApplication.Business.LoginService
{
    public class LoginService
    {
        private readonly ILoginRepository repository;

        public LoginService(ILoginRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            Dictionary<string, string> errors;
            try
            {
                errors = LoginValidator.ValidateCredentials(username, password);
            }
            catch (Exception ex)
            {
                throw new Exception("Error during validation: " + ex.Message);
            }

            if (errors.Count != 0)
            {
                throw new LoginValidationException(errors);
            }

            return await repository.ValidateUserAsync(username, password);
        }
    }
}
