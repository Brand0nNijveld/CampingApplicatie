using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business
{
    public class CampingSpot(int ID, double x, double y) : MapEntity(ID, x, y)
    {
        public double Width { get; private set; } = 10;
        public double Height { get; private set; } = 7;

        public List<Booking> Bookings { get; private set; } = [];

        public CampingSpot(int ID, double x, double y, Booking[] bookings) : this(ID, x, y)
        {
            Bookings = new(bookings);
        }

        public CampingSpot(int ID, double x, double y, double width, double height) : this(ID, x, y)
        {
            Width = width;
            Height = height;
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

        public (double x, double y) GetCenterPoint()
        {
            return ((XCoordinate + Width / 2), (YCoordinate + Height / 2));
        }
    }
}
