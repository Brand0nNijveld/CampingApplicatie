using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CampingApplication.Business;

namespace CampingApplication.VisitorApp.ViewModels
{
    public class CampingMapViewModel() : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<CampingSpot> campingSpots = [];
        public ObservableCollection<CampingSpot> CampingSpots
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

        public CampingMapViewModel(string backgroundImage, CampingSpot[] campingSpots) : this()
        {
            this.campingSpots = new(campingSpots);
            this.backgroundImage = backgroundImage;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this.PropertyChanged, new PropertyChangedEventArgs(propertyName));
        }
    }
}
