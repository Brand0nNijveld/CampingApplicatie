using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business
{
    public class CampingSpot
    {
        public int ID { get; private set; }
        public List<Booking> Bookings { get; private set; } = [];

        public CampingSpot()
        {

        }

        public CampingSpot(int ID, Booking[] bookings)
        {
            this.ID = ID;
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
