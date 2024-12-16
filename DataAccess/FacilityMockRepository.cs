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
                new Facility(1, "Toilet", 382, 42)
                ];
        }
    }
}
