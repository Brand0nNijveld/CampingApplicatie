using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.FacilityService
{
    public class FacilityService
    {
        private readonly IFacilityRepository repository;

        public FacilityService(IFacilityRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<Facility>> GetFacilitiesAsync()
        {
            var facilities = await repository.GetFacilitiesAsync();
            return facilities.ToList();
        }
    }
}
