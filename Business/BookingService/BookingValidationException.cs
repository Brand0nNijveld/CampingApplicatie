using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.BookingService
{
    public class BookingValidationException : Exception
    {
        public Dictionary<string, string> Errors { get; private set; }

        public BookingValidationException(Dictionary<string, string> errors)
        {
            Errors = errors;
        }
    }
}
