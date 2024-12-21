using CampingApplication.Business;
using CampingApplication.Client.Shared.Helpers;
using CampingApplication.VisitorApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.VisitorApp.ViewModels
{
    public class CampingSpotViewModel : BaseViewModel
    {
        private readonly CampingMapModel campingMapModel;

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

        public CampingSpotViewModel(CampingMapModel campingMapModel, CampingSpot spot)
        {
            ID = spot.ID;

            double pixelsPerMeter = CampingMapViewModel.PIXELS_PER_METER;
            PositionX = MapConversionHelper.MetersToPixels(spot.XCoordinate, pixelsPerMeter);
            PositionY = MapConversionHelper.MetersToPixels(spot.YCoordinate, pixelsPerMeter);

            Width = MapConversionHelper.MetersToPixels(spot.Width, pixelsPerMeter);
            Height = MapConversionHelper.MetersToPixels(spot.Height, pixelsPerMeter);

            this.campingMapModel = campingMapModel;
            campingMapModel.PropertyChanged += CampingMapModel_PropertyChanged;
        }

        private void CampingMapModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(campingMapModel.AvailableCampingSpots))
            {
                if (campingMapModel.AvailableCampingSpots == null)
                {
                    Available = false;
                }
                else
                {
                    Available = campingMapModel.AvailableCampingSpots.ContainsKey(ID);
                }
            }
        }

        public void SelectCampingSpot()
        {
            if (campingMapModel == null)
                return;

            SpotInformationModel? selectedSpot = campingMapModel.SelectedCampingSpot;

            // Check if this camping spot is already selected
            if (selectedSpot != null)
            {
                if (selectedSpot.CampingSpot.ID == ID)
                    return;
            }

            campingMapModel.SelectedCampingSpot = new(campingMapModel.CampingSpots[ID]);
        }
    }
}
