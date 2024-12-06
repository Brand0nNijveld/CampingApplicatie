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
            _ = GetCampingSpotsAsync();
        }

        private async Task GetCampingSpotsAsync()
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
            SetAvailability([]);
        }

        public void SetAvailability(Dictionary<int, CampingSpot> available)
        {
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
            BookingView bookingView = new(ID, DateTime.Now, DateTime.Now.AddDays(5), 60);
            bookingView.BackButtonClicked += () => actionPanelViewModel.ClearAndHide();
            bookingView.ViewModel.BookingSuccessful += () => actionPanelViewModel.CurrentView = 1;
            BookingSuccessView bookingSuccessView = new();
            bookingSuccessView.DoneButtonClicked += () => actionPanelViewModel.ClearAndHide();

            actionPanelViewModel.SetSteps([bookingView, bookingSuccessView]);
        }
    }
}
