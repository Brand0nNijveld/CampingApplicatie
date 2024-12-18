﻿using System;
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
using CampingApplication.VisitorApp.Views.Booking;

namespace CampingApplication.VisitorApp.ViewModels
{
    public delegate void MapLoadHandler();
    public delegate void MapLoadErrorHandler();
    public delegate void AvailabilityHandler(bool available);
    public class CampingMapViewModel : INotifyPropertyChanged
    {
        public const int PIXELS_PER_METER = 25;

        public event AvailabilityHandler? AvailabilityChanged;
        public event MapLoadHandler? MapLoaded;
        public event MapLoadErrorHandler? MapLoadError;

        public event PropertyChangedEventHandler? PropertyChanged;

        private CampingSpotService campingSpotService;
        private FacilityService facilityService;

        public List<Facility> FacilityData { get; private set; } = [];
        public List<FacilityViewModel> Facilities = [];

        public List<CampingSpot> CampingSpotData { get; private set; } = [];
        public List<CampingSpotViewModel> CampingSpots = [];

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

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

        public CampingMapViewModel(ActionPanelViewModel actionPanelViewModel)
        {
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
                CampingSpotData = await getCampingSpots;
                FacilityData = await getFacilities;

                var campingSpotViewModels = new List<CampingSpotViewModel>();
                foreach (var spot in CampingSpotData)
                {
                    campingSpotViewModels.Add(new(this, spot));
                }

                var facilityViewModels = new List<FacilityViewModel>();
                foreach (var facility in FacilityData)
                {
                    facilityViewModels.Add(new(facility));
                }

                CampingSpots = campingSpotViewModels;
                Facilities = facilityViewModels;

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
            MapWidthInMeters = MapConversionHelper.PixelsToMeters(widthInPixels, PIXELS_PER_METER);
            MapHeightInMeters = MapConversionHelper.PixelsToMeters(heightInPixels, PIXELS_PER_METER);
        }

        public void ClearAvailability()
        {
            SetAvailability([], DateTime.Now, DateTime.Now);
        }

        public void SetAvailability(Dictionary<int, CampingSpot> available, DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;

            if (available.Count == 0)
            {
                AvailabilityChanged?.Invoke(false);
            }

            foreach (var visual in CampingSpots)
            {
                if (available.ContainsKey(visual.ID))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        visual.Available = true;
                    });
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        visual.Available = false;
                    });
                }
            }

            AvailabilityChanged?.Invoke(true);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this.PropertyChanged, new PropertyChangedEventArgs(propertyName));
        }

        public void ShowBookScreen(int ID)
        {
            if (StartDate >= EndDate)
                return;

            BookingView bookingView = new(ID, StartDate, EndDate, 60);
            bookingView.BackButtonClicked += () => actionPanelViewModel.ClearAndHide();
            bookingView.ViewModel.BookingSuccessful += () =>
            {
                actionPanelViewModel.CurrentView = 1;
                GetAvailability();
            };
            BookingSuccessView bookingSuccessView = new();
            bookingSuccessView.DoneButtonClicked += () => actionPanelViewModel.ClearAndHide();

            actionPanelViewModel.SetSteps([bookingView, bookingSuccessView]);
        }

        public async void GetAvailability()
        {
            try
            {
                var availableSpots = await campingSpotService.GetAvailableSpotsAsync(StartDate, EndDate);

                Dictionary<int, CampingSpot> availableDict = [];
                foreach (var available in availableSpots)
                {
                    availableDict.TryAdd(available.ID, available);
                }

                Debug.WriteLine($"{availableDict.Count} camping spots available");

                SetAvailability(availableDict, StartDate, EndDate);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
