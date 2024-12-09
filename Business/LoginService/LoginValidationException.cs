using System;
using System.Collections.Generic;

namespace CampingApplication.Business.LoginService
{
    public class LoginValidationException : Exception
    {
        public Dictionary<string, string> Errors { get; private set; }

        public LoginValidationException(Dictionary<string, string> errors)
        {
            Errors = errors;
        }
    }
}
