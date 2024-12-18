using CampingApplication.Business;
using CampingApplication.Business.FacilityService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class FacilityMockRepository : IFacilityRepository
    {
        public async Task<IEnumerable<Facility>> GetFacilitiesAsync()
        {
            await Task.Delay(100);
            return [
                new Facility(1,  34, 6, FacilityType.Restroom),
                new Facility(2, 20, 20,FacilityType.Shower),
                new Facility(3,  10, 10, FacilityType.Playground),
                new Facility(4,  30, 42, FacilityType.Reception),
                ];
        }
    }
}
