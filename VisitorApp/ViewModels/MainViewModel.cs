using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CampingApplication.Business;
using CampingApplication.Business.CampingSpotService;
using CampingApplication.VisitorApp.Views.Booking;
using CampingApplication.VisitorApp.Models;

namespace CampingApplication.VisitorApp.ViewModels
{
    public class MainViewModel
    {
        public CampingSpotService CampingSpotService { get; private set; }
        public CampingMapViewModel CampingMapViewModel { get; private set; }

        public ActionPanelViewModel ActionPanelViewModel { get; private set; }

        public MainViewModel()
        {
            CampingSpotService = ServiceProvider.Current.Resolve<CampingSpotService>();

            ActionPanelViewModel = new();
            CampingMapViewModel = new(ActionPanelViewModel);
        }

        public void CheckAvailableSpots(DateTime startDate, DateTime endDate)
        {
            var availableSpots = CampingSpotService.GetAvailableSpots([.. CampingMapViewModel.CampingSpotData], startDate, endDate);

            Dictionary<int, CampingSpot> availableDict = [];
            foreach (var available in availableSpots)
            {
                availableDict.TryAdd(available.ID, available);
            }

            Debug.WriteLine($"{availableDict.Count} camping spots available");

            CampingMapViewModel.SetAvailability(availableDict, startDate, endDate);
        }
    }
}
