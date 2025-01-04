﻿using CampingApplication.Business;
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
                new Facility(1,  135.2, 173.44, FacilityType.Restroom),
                new Facility(2, 190.4, 23.2, FacilityType.Shower),
                new Facility(3,  134.24, 92.16, FacilityType.Playground),
                new Facility(4,  108.96, 225.12, FacilityType.Reception),
                new Facility(5,  54.56, 100, FacilityType.Restroom),
                ];
        }
    }
}
