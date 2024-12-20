using CampingApplication.Business.CampingSpotService;
using CampingApplication.Business;
using CampingApplication.VisitorApp.Models;
using CampingApplication.VisitorApp.Views.Information;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace CampingApplication.VisitorApp.ViewModels
{
    public delegate void AvailabilityHandler(bool available);

    public class CampingMapViewModel : INotifyPropertyChanged
    {
        public event AvailabilityHandler? AvailabilityChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public List<CampingSpot> CampingSpotData { get; private set; }

        private ObservableCollection<CampingSpotVisualModel> campingSpots = new();
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

        private int numberOfNights;
        public int NumberOfNights
        {
            get => numberOfNights;
            set
            {
                numberOfNights = value;
                OnPropertyChanged(nameof(NumberOfNights));
            }
        }

        public void UpdateNumberOfNights(DateTime startDate, DateTime endDate)
        {
            NumberOfNights = (endDate - startDate).Days;
        }

        public CampingMapViewModel(ActionPanelViewModel actionPanelViewModel)
        {
            this.actionPanelViewModel = actionPanelViewModel;
            var campingSpotService = ServiceProvider.Current.Resolve<CampingSpotService>();
            var spots = campingSpotService.GetCampingSpots();
            CampingSpotData = spots;

            var campingSpotVisuals = new CampingSpotVisualModel[spots.Count];
            for (int i = 0; i < spots.Count; i++)
            {
                CampingSpot spot = spots[i];
                campingSpotVisuals[i] = new CampingSpotVisualModel(spot.ID, spot.PositionX, spot.PositionY);
            }

            CampingSpots = new ObservableCollection<CampingSpotVisualModel>(campingSpotVisuals);
        }

        public void ClearAvailability()
        {
            SetAvailability(new Dictionary<int, CampingSpot>());
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ShowBookScreen(int ID, DateTime startDate, DateTime endDate)
        {
            // Calculate the number of nights
            UpdateNumberOfNights(startDate, endDate);

            // Create SpotInfo UserControl, passing in the ID and NumberOfNights
            var spotInfo = new SpotInfo(ID, NumberOfNights);

            // Pass the SpotInfo UserControl to the action panel
            actionPanelViewModel.SetSteps(new List<UserControl> { spotInfo });
        }
    }
}
