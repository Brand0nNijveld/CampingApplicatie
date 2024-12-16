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
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public CampingSpotInfo Info { get; set; }
        public List<Booking> Bookings { get; private set; } = [];


        public CampingSpot(int ID, int x, int y)
        {
            this.ID = ID;
            this.PositionX = x;
            this.PositionY = y;
        }

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
