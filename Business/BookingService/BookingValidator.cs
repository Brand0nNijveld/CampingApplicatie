using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.BookingService
{
    public class BookingValidator
    {
        public static Dictionary<string, string> ValidateRequest(BookingRequest request)
        {
            Dictionary<string, string> errors = [];

            string? firstNameError = ValidateFirstName(request.FirstName);
            if (firstNameError != null)
            {
                errors.TryAdd(nameof(request.FirstName), firstNameError);
            }

            string? lastNameError = ValidateLastName(request.LastName);
            if (lastNameError != null)
            {
                errors[nameof(request.LastName)] = lastNameError;
            }

            string? emailError = ValidateEmail(request.Email);
            if (emailError != null)
            {
                errors[nameof(request.Email)] = emailError;
            }

            string? phoneNumberError = ValidatePhoneNumber(request.PhoneNumber);
            if (phoneNumberError != null)
            {
                errors[nameof(request.PhoneNumber)] = phoneNumberError;
            }

            return errors;
        }

        public static string? ValidateFirstName(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "Voornaam is verplicht.";

            if (str.Length < 3)
                return "Voornaam is te kort.";

            if (str.Length > 30)
                return "Voornaam is te lang.";

            return null;
        }

        public static string? ValidateLastName(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "Achternaam is verplicht.";

            if (str.Length < 3)
                return "Achternaam is te kort.";

            if (str.Length > 50)
                return "Achternaam is te lang.";

            return null;
        }

        public static string? ValidateEmail(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "E-mailadres is verplicht.";

            if (!System.Text.RegularExpressions.Regex.IsMatch(str, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "Ongeldig e-mailadres.";

            return null;
        }

        public static string? ValidatePhoneNumber(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "Telefoonnummer is verplicht.";

            if (!System.Text.RegularExpressions.Regex.IsMatch(str, @"^\+?[0-9\s\-()]{7,15}$"))
                return "Ongeldig telefoonnummer.";

            return null;
        }

    }
}
