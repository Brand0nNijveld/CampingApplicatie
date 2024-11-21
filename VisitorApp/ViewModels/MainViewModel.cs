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
