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

        public List<CampingSpot> GetCampingSpots()
        {
            return repository.GetCampingSpots().ToList();
        }

        public IEnumerable<CampingSpot> GetAvailableSpots(CampingSpot[] spots, DateTime startDate, DateTime endDate)
        {
            return repository.GetAvailableSpots(spots, startDate, endDate);
        }
    }
}
