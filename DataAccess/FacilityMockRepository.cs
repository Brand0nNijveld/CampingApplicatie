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
                new Facility(1, FacilityType.Restroom, 382, 42),
                new Facility(2, FacilityType.Shower, 382, 80),
                new Facility(3, FacilityType.Playground, 382, 160),
                new Facility(4, FacilityType.Reception, 343, 378),
                ];
        }
    }
}
