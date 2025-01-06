using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CampingApplication.Business;
using CampingApplication.Business.CampingSpotService;
using CampingApplication.Business.FacilityService;
using CampingApplication.Client.Shared.Helpers;
using CampingApplication.VisitorApp.Models;
using CampingApplication.VisitorApp.Views.Booking;

namespace CampingApplication.VisitorApp.ViewModels
{
    public delegate void MapLoadHandler();
    public delegate void MapLoadErrorHandler();
    public delegate void AvailabilityHandler(bool available);
    public class CampingMapViewModel : BaseViewModel
    {
        public const double PIXELS_PER_METER = 6.25;

        public event AvailabilityHandler? AvailabilityChanged;
        public event MapLoadHandler? MapLoaded;
        public event MapLoadErrorHandler? MapLoadError;

        private CampingSpotService campingSpotService;
        private FacilityService facilityService;

        public CampingMapModel Model { get; private set; }

        public List<Facility> Facilities
        {
            get => Model.Facilities; set
            {
                Model.Facilities = value;
                OnPropertyChanged(nameof(Facilities));
            }
        }

        public Dictionary<int, CampingSpot> CampingSpots
        {
            get => Model.CampingSpots;
            set
            {
                Model.CampingSpots = value;
                OnPropertyChanged(nameof(CampingSpots));
            }
        }

        private ActionPanelViewModel actionPanelViewModel;

        private double mapWidthInMeters = 0;
        public double MapWidthInMeters
        {
            get => mapWidthInMeters;
            set
            {
                mapWidthInMeters = value;
                OnPropertyChanged(nameof(MapWidthInMeters));
            }
        }

        private double mapHeightInMeters = 0;
        public double MapHeightInMeters
        {
            get => mapHeightInMeters;
            set
            {
                mapHeightInMeters = value;
                OnPropertyChanged(nameof(MapHeightInMeters));
            }
        }

        public CampingMapViewModel(CampingMapModel model, ActionPanelViewModel actionPanelViewModel)
        {
            this.Model = model;
            this.actionPanelViewModel = actionPanelViewModel;
            campingSpotService = ServiceProvider.Current.Resolve<CampingSpotService>();
            facilityService = ServiceProvider.Current.Resolve<FacilityService>();
        }

        public async Task GetMapDataAsync()
        {
            try
            {
                var getCampingSpots = campingSpotService.GetCampingSpotsAsync();
                var getFacilities = facilityService.GetFacilitiesAsync();
                await Task.WhenAll(getCampingSpots, getFacilities);

                var campingSpots = await getCampingSpots;
                Facilities = await getFacilities;

                foreach (var spot in campingSpots)
                {
                    CampingSpots.TryAdd(spot.ID, spot);
                }

                MapLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MapLoadError?.Invoke();
            }
        }

        public void SetWidthAndHeight(double widthInPixels, double heightInPixels)
        {
            var (width, height) = MapConversionHelper.PixelsToMeters(widthInPixels, heightInPixels, PIXELS_PER_METER);
            MapWidthInMeters = width;
            MapHeightInMeters = height;
        }

        public void ClearAvailability()
        {
            SetAvailability([]);
        }

        public void SetAvailability(Dictionary<int, CampingSpot> available)
        {
            if (available.Count == 0)
            {
                AvailabilityChanged?.Invoke(false);
            }

            Model.AvailableCampingSpots = available;

            AvailabilityChanged?.Invoke(true);
        }

        public void SelectCampingSpot(CampingSpot? campingSpot)
        {

            //if (StartDate >= EndDate)
            //    return;

            //int ID = campingSpot.ID;

            //BookingView bookingView = new(ID, StartDate, EndDate, 60);
            //bookingView.BackButtonClicked += () => actionPanelViewModel.ClearAndHide();
            //bookingView.ViewModel.BookingSuccessful += () =>
            //{
            //    actionPanelViewModel.Next();
            //    GetAvailability();
            //};
            //BookingSuccessView bookingSuccessView = new();
            //bookingSuccessView.DoneButtonClicked += () => actionPanelViewModel.ClearAndHide();

            //actionPanelViewModel.SetSteps([bookingView, bookingSuccessView]);
        }

        public async void GetAvailability()
        {
            try
            {
                var availableSpots = await campingSpotService.GetAvailableSpotsAsync(Model.StartDate, Model.EndDate);

                Dictionary<int, CampingSpot> availableDict = [];
                foreach (var available in availableSpots)
                {
                    availableDict.TryAdd(available.ID, available);
                }

                Debug.WriteLine($"{availableDict.Count} camping spots available");

                SetAvailability(availableDict);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
