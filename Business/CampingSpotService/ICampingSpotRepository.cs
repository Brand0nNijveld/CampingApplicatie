using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.CampingSpotService
{
    public interface ICampingSpotRepository
    {
        public Task<IEnumerable<CampingSpot>> GetCampingSpotsAsync();
        public CampingSpot GetCampingSpot(int ID);

        public IEnumerable<CampingSpot> GetAvailableSpots(CampingSpot[] spots, DateTime startDate, DateTime endDate);
        public void SaveCampingSpots(IEnumerable<CampingSpot> spots);
        public void AddCampingSpot(int ID, int X, int Y);
        public Task<IEnumerable<CampingSpot>> GetAvailableSpotsAsync(DateTime startDate, DateTime endDate);
    }
}
