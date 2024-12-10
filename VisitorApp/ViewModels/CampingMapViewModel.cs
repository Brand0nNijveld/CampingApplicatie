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
using CampingApplication.VisitorApp.Models;
using CampingApplication.VisitorApp.Views.Booking;

namespace CampingApplication.VisitorApp.ViewModels
{
    public delegate void AvailabilityHandler(bool available);
    public class CampingMapViewModel : INotifyPropertyChanged
    {
        public event AvailabilityHandler? AvailabilityChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        private CampingSpotService campingSpotService;

        public List<CampingSpot> CampingSpotData { get; private set; } = [];

        private ObservableCollection<CampingSpotVisualModel> campingSpots = [];
        public ObservableCollection<CampingSpotVisualModel> CampingSpots
        {
            get => campingSpots;
            set
            {
                campingSpots = value;
                OnPropertyChanged(nameof(CampingSpots));
            }
        }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        private ActionPanelViewModel actionPanelViewModel;

        private string backgroundImage = "";
        public string BackgroundImage
        {
            get => backgroundImage;
            set
            {
                backgroundImage = value;
                OnPropertyChanged(nameof(BackgroundImage));
            }
        }

        public CampingMapViewModel(ActionPanelViewModel actionPanelViewModel)
        {
            this.actionPanelViewModel = actionPanelViewModel;
            campingSpotService = ServiceProvider.Current.Resolve<CampingSpotService>();
        }

        public async Task GetCampingSpotsAsync()
        {
            try
            {
                var spots = await campingSpotService.GetCampingSpotsAsync();
                CampingSpotData = spots;

                var campingSpotVisuals = new CampingSpotVisualModel[spots.Count];
                for (int i = 0; i < spots.Count; i++)
                {
                    Debug.WriteLine("Camping spot: " + spots[i].ID);
                    CampingSpot spot = spots[i];
                    campingSpotVisuals[i] = new(spot.ID, spot.PositionX, spot.PositionY);
                }

                // Use Dispatcher to update UI-bound properties or raise events
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CampingSpots = new ObservableCollection<CampingSpotVisualModel>(campingSpotVisuals);
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
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

            foreach (var visual in campingSpots)
            {
                if (available.ContainsKey(visual.ID))
                {
                    visual.Available = true;
                }
                else
                {
                    visual.Available = false;
                }
            }

            OnPropertyChanged(nameof(CampingSpots));
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
                actionPanelViewModel.Next();
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
