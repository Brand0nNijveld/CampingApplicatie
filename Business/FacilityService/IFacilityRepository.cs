using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.FacilityService
{
    public interface IFacilityRepository
    {
        public Task<IEnumerable<Facility>> GetFacilitiesAsync();
    }
}
