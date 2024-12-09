using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.CampingSpotService
{
    public class CampingSpotService
    {
        private ICampingSpotRepository repository;
        public CampingSpotService(ICampingSpotRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<CampingSpot>> GetCampingSpots()
        {
            var spots = await repository.GetCampingSpotsAsync();
            return spots.ToList();
        }

        public async Task<List<CampingSpot>> GetCampingSpotsAsync()
        {
            var spots = await repository.GetCampingSpotsAsync();
            return spots.ToList();
        }

        public async Task<IEnumerable<CampingSpot>> GetAvailableSpotsAsync(DateTime startDate, DateTime endDate)
        {
            return await repository.GetAvailableSpotsAsync(startDate, endDate);
        }

        // Assumes the caming spots has the booking data in memory.
        public static IEnumerable<CampingSpot> GetAvailableSpots(CampingSpot[] spots, DateTime startDate, DateTime endDate)
        {
            return spots.Where(c => c.IsAvailableDuringPeriod(startDate, endDate));
        }
    }
}
