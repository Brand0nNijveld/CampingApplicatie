using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CampingApplication.Business;
using CampingApplication.EmployeeApp.Models;

namespace CampingApplication.EmployeeApp.ViewModels
{
    public delegate void AvailabilityHandler(bool available);
    public class CampingMapViewModel() : INotifyPropertyChanged
    {
        public event AvailabilityHandler? AvailabilityChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

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

        public CampingMapViewModel(string backgroundImage, List<CampingSpot> campingSpots) : this()
        {
            var campingSpotVisuals = new CampingSpotVisualModel[campingSpots.Count];
            for (int i = 0; i < campingSpots.Count; i++)
            {
                CampingSpot spot = campingSpots[i];
                campingSpotVisuals[i] = new(spot.ID, spot.XCoordinate, spot.YCoordinate);
            }

            this.campingSpots = new(campingSpotVisuals);
            this.backgroundImage = backgroundImage;
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
    }
}
