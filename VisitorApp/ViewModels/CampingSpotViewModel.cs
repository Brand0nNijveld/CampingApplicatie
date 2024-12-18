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

        private int width;
        public int Width
        {
            get => width;
            set
            {
                width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        private int height;
        public int Height
        {
            get => height;
            set
            {
                height = value;
                OnPropertyChanged(nameof(Height));
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
            ID = spot.ID;

            double pixelsPerMeter = CampingMapViewModel.PIXELS_PER_METER;
            PositionX = MapConversionHelper.MetersToPixels(spot.XCoordinate, pixelsPerMeter);
            PositionY = MapConversionHelper.MetersToPixels(spot.YCoordinate, pixelsPerMeter);

            Width = MapConversionHelper.MetersToPixels(spot.Width, pixelsPerMeter);
            Height = MapConversionHelper.MetersToPixels(spot.Height, pixelsPerMeter);

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
