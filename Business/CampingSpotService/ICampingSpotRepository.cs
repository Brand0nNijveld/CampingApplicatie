﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.CampingSpotService
{
    public interface ICampingSpotRepository
    {
        public IEnumerable<CampingSpot> GetCampingSpots();
        public IEnumerable<CampingSpot> GetAvailableSpots(CampingSpot[] spots, DateTime startDate, DateTime endDate);
        public CampingSpot GetCampingSpot(int ID);
    }
}
