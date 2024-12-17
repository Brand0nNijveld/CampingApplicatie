﻿using System;
using System.Collections.Generic;

namespace CampingApplication.Business.LoginService
{
    public class LoginValidator
    {
        public static Dictionary<string, string> ValidateCredentials(string username, string password)
        {
            var errors = new Dictionary<string, string>();

            string? usernameError = ValidateUsername(username);
            if (usernameError != null)
            {
                errors[nameof(username)] = usernameError;
            }

            string? passwordError = ValidatePassword(password);
            if (passwordError != null)
            {
                errors[nameof(password)] = passwordError;
            }

            return errors;
        }

        public static string? ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return "Gebruikersnaam is verplicht.";

            if (username.Length < 3)
                return "Gebruikersnaam is te kort.";

            if (username.Length > 20)
                return "Gebruikersnaam is te lang.";

            return null;
        }

        public static string? ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return "Wachtwoord is verplicht.";

            if (password.Length < 3)
                return "Wachtwoord moet minstens 3 tekens bevatten.";

            return null;
        }
    }
}
