using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<List<CampingSpot>> GetCampingSpotsAsync()
        {
            var spots = await repository.GetCampingSpotsAsync();
            return spots.ToList();
        }

        public async Task<IEnumerable<CampingSpot>> GetAvailableSpotsAsync(DateTime startDate, DateTime endDate)
        {
            return await repository.GetAvailableSpotsAsync(startDate, endDate);
        }

        // Assumes the camping spots have the booking data in memory.
        public static IEnumerable<CampingSpot> GetAvailableSpots(CampingSpot[] spots, DateTime startDate, DateTime endDate)
        {
            return spots.Where(c => c.IsAvailableDuringPeriod(startDate, endDate));
        }

        public void SaveCampingSpots(IEnumerable<CampingSpot> spots)
        {
            repository.SaveCampingSpots(spots);
            Debug.WriteLine("campingspots opslaan");
        }

        public void AddCampingSpot(int ID, int X, int Y)
        {
            repository.AddCampingSpot(ID, X, Y);
        }
    }
}
