using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business
{
    public class CampingAvailabilityService
    {
        private CampingSpot[] campingSpots;

        /// <summary>
        /// Assumes all campingspots with their future bookings have been fetched from the database.
        /// </summary>
        /// <param name="campingSpots"></param>
        public CampingAvailabilityService(CampingSpot[] campingSpots)
        {
            this.campingSpots = campingSpots;
        }

        public IEnumerable<CampingSpot> GetAvailableCampingSpots(DateTime startDate, DateTime endDate)
        {
            return GetAvailableCampingSpots(campingSpots, startDate, endDate);
        }

        public static IEnumerable<CampingSpot> GetAvailableCampingSpots(CampingSpot[] campingSpots, DateTime startDate, DateTime endDate)
        {
            return campingSpots.Where(c => c.IsAvailableDuringPeriod(startDate, endDate));
        }
    }
}
