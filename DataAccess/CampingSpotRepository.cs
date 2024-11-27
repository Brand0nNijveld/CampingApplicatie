using CampingApplication.Business;
using CampingApplication.Business.CampingSpotService;

namespace DataAccess
{
    public class CampingSpotRepository : ICampingSpotRepository
    {
        public IEnumerable<CampingSpot> GetAvailableSpots(CampingSpot[] spots, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public CampingSpot GetCampingSpot(int ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CampingSpot> GetCampingSpots()
        {
            throw new NotImplementedException();
        }
    }
}
