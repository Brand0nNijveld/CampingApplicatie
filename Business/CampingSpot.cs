using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business
{
    public class CampingSpot(int ID, double x, double y) : MapEntity(ID, x, y)
    {
        public List<Booking> Bookings { get; private set; } = [];

        public CampingSpot(int ID, int x, int y, Booking[] bookings) : this(ID, x, y)
        {
            Bookings = new(bookings);
        }

        public bool IsAvailableDuringPeriod(DateTime startDate, DateTime endDate)
        {
            return !Bookings.Where(
                b =>
                b.StartDate >= startDate &&
                b.StartDate <= endDate ||
                b.EndDate >= startDate &&
                b.EndDate <= endDate).Any();
        }
    }
}
