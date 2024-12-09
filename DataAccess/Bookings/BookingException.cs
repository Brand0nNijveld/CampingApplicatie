using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Bookings
{
    public enum BookingExceptionType
    {
        AlreadyBooked,
    }

    public class BookingException(string message, BookingExceptionType type) : Exception(message)
    {
        public BookingExceptionType Type { get; private set; } = type;
    }
}
