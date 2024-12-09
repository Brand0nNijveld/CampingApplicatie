using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.CampingSpotService
{
    public class CampingSpotMockRepository : ICampingSpotRepository
    {
        public string AddCampingSpot(int ID, int X, int Y)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CampingSpot> GetAvailableSpots(CampingSpot[] spots, DateTime startDate, DateTime endDate)
        {
            return spots.Where(c => c.IsAvailableDuringPeriod(startDate, endDate));
        }

        public CampingSpot GetCampingSpot(int ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CampingSpot> GetCampingSpots()
        {
            return [
                    new(1, 612, 360, [new(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6))]),
                    new(2, 612, 281, [new(DateTime.Now.AddDays(12), DateTime.Now.AddDays(22))]),
                    new(3, 612, 201,[new(DateTime.Now.AddDays(3), DateTime.Now.AddDays(12))]),
                    new(4, 612, 115, [new(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6))]),
                    new(5, 612, 30, [new(DateTime.Now.AddDays(12), DateTime.Now.AddDays(22))]),
                    new(6, 322, 227,[new(DateTime.Now.AddDays(3), DateTime.Now.AddDays(12))]),
                    new(7, 322, 145, [new(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6))]),
                    new(8, 86, 186, [new(DateTime.Now.AddDays(12), DateTime.Now.AddDays(22))]),
                    new(9, 86, 103, [new(DateTime.Now.AddDays(12), DateTime.Now.AddDays(22))]),
                    new(10, 86, 29, [new(DateTime.Now.AddDays(12), DateTime.Now.AddDays(22))]),
                    new(11, 190, 29, [new(DateTime.Now.AddDays(12), DateTime.Now.AddDays(22))]),
                ];
        }
    }
}
