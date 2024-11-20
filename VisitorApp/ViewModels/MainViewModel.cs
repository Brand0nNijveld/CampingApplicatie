using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CampingApplication.Business;
using CampingApplication.VisitorApp.Models;

namespace CampingApplication.VisitorApp.ViewModels
{
    public class MainViewModel
    {
        public CampingMapViewModel CampingMapViewModel { get; private set; }
        private List<CampingSpot> campingSpots = [];

        public MainViewModel()
        {
            campingSpots =
                [
                    new(0, 188, 32, [new(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6))]),
                    new(1, 268, 32, [new(DateTime.Now.AddDays(12), DateTime.Now.AddDays(22))]),
                    new(2, 348, 32,[new(DateTime.Now.AddDays(3), DateTime.Now.AddDays(12))]),
                ];

            CampingSpotVisualModel[] campingSpotVisuals = new CampingSpotVisualModel[campingSpots.Count];
            for (int i = 0; i < campingSpots.Count; i++)
            {
                CampingSpot spot = campingSpots[i];
                campingSpotVisuals[i] = new(spot.ID, spot.PositionX, spot.PositionY);
            }

            CampingMapViewModel = new("../test1.png", campingSpotVisuals);
        }

        public void CheckAvailableSpots(DateTime beginDate, DateTime endDate)
        {
            var availableSpots = AvailabilityService.GetAvailableCampingSpots([.. campingSpots], beginDate, endDate);

            Dictionary<int, CampingSpot> availableDict = [];
            foreach (var available in availableSpots)
            {
                availableDict.TryAdd(available.ID, available);
            }

            Debug.WriteLine($"{availableDict.Count} camping spots available");

            CampingMapViewModel.SetAvailability(availableDict);
        }
    }
}
