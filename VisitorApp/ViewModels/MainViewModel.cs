using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CampingApplication.Business;
using CampingApplication.Business.CampingSpotService;
using CampingApplication.VisitorApp.Models;
using CampingApplication.VisitorApp.Views.Booking;

namespace CampingApplication.VisitorApp.ViewModels
{
    public class MainViewModel
    {
        public CampingSpotService CampingSpotService { get; private set; }
        public CampingMapViewModel CampingMapViewModel { get; private set; }

        public ActionPanelViewModel ActionPanelViewModel { get; private set; }
        public SpotInformationViewModel SpotInformationViewModel { get; private set; }
        public PathViewModel PathViewModel { get; private set; }
        private CampingMapModel campingMapModel;

        public MainViewModel()
        {
            CampingSpotService = ServiceProvider.Current.Resolve<CampingSpotService>();
            campingMapModel = new();

            ActionPanelViewModel = new(campingMapModel);
            CampingMapViewModel = new(campingMapModel, ActionPanelViewModel);
            PathViewModel = new(campingMapModel);
            SpotInformationViewModel = new(campingMapModel);

            SpotInformationViewModel.BookingProcessStarted += OnBookingProcessStarted;
        }

        public void SetDates(DateTime start, DateTime end)
        {
            campingMapModel.StartDate = start;
            campingMapModel.EndDate = end;
        }

        public async void CheckAvailableSpots(DateTime startDate, DateTime endDate)
        {
            try
            {
                var availableSpots = await CampingSpotService.GetAvailableSpotsAsync(startDate, endDate);

                Dictionary<int, CampingSpot> availableDict = [];
                foreach (var available in availableSpots)
                {
                    availableDict.TryAdd(available.ID, available);
                }

                Debug.WriteLine($"{availableDict.Count} camping spots available");

                CampingMapViewModel.SetAvailability(availableDict);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }


        public void OnBookingProcessStarted()
        {
            if (ActionPanelViewModel.Views.Count > 0)
                return;

            var startDate = campingMapModel.StartDate;
            var endDate = campingMapModel.EndDate;
            if (startDate >= endDate)
                return;

            var campingSpot = campingMapModel.SelectedCampingSpot;
            if (campingSpot == null)
            {
                ActionPanelViewModel.ClearAndHide();
                return;
            }

            int ID = campingSpot.CampingSpot.ID;

            BookingView bookingView = new(ID, campingMapModel);
            bookingView.BackButtonClicked += ActionPanelViewModel.ClearAndHide;
            bookingView.ViewModel.BookingSuccessful += () =>
            {
                ActionPanelViewModel.Next();
                campingMapModel.AvailableCampingSpots?.Remove(ID);
                CheckAvailableSpots(campingMapModel.StartDate, campingMapModel.EndDate);
            };

            BookingSuccessView bookingSuccessView = new();
            bookingSuccessView.DoneButtonClicked += () =>
            {
                ActionPanelViewModel.ClearAndHide();
                campingMapModel.SelectedCampingSpot = null;
            };

            ActionPanelViewModel.SetSteps([bookingView, bookingSuccessView]);
        }
    }
}
