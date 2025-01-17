﻿using CampingApplication.Business;
using CampingApplication.Client.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.VisitorApp.ViewModels
{
    public class FacilityViewModel : BaseViewModel
    {
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

        private string type;
        public string Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public FacilityViewModel(Facility facility)
        {
            ID = facility.ID;
            double pixelsPerMeter = CampingMapViewModel.PIXELS_PER_METER;
            positionX = MapConversionHelper.MetersToPixels(facility.XCoordinate, pixelsPerMeter);
            positionY = MapConversionHelper.MetersToPixels(facility.YCoordinate, pixelsPerMeter);

            type = facility.Type.ToString();
        }
    }
}
