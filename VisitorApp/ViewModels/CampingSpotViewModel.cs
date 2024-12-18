using CampingApplication.Business;
using CampingApplication.Client.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.VisitorApp.ViewModels
{
    public class CampingSpotViewModel : INotifyPropertyChanged
    {
        private readonly CampingMapViewModel campingMapViewModel;

        private int id;
        public int ID
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        private int positionX;
        public int PositionX
        {
            get => positionX;
            set
            {
                positionX = value;
                OnPropertyChanged(nameof(PositionX));
            }
        }

        private int positionY;
        public int PositionY
        {
            get => positionY;
            set
            {
                positionY = value;
                OnPropertyChanged(nameof(PositionY));
            }
        }

        private bool available;
        public bool Available
        {
            get => available;
            set
            {
                available = value;
                OnPropertyChanged(nameof(Available));
            }
        }

        public CampingSpotViewModel(CampingMapViewModel campingMapViewModel, CampingSpot spot)
        {
            this.ID = spot.ID;
            int pixelsPerMeter = CampingMapViewModel.PIXELS_PER_METER;
            this.PositionX = MapConversionHelper.MetersToPixels(spot.XCoordinate, pixelsPerMeter);
            this.PositionY = MapConversionHelper.MetersToPixels(spot.YCoordinate, pixelsPerMeter);
            this.campingMapViewModel = campingMapViewModel;
        }

        public void ShowDetails()
        {
            campingMapViewModel.ShowBookScreen(ID);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this.PropertyChanged, new PropertyChangedEventArgs(propertyName));
        }
    }
}
