using CampingApplication.Business.CampingSpotService;
using CampingApplication.Business;
using CampingApplication.VisitorApp.Views.Information;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CampingApplication.VisitorApp.Models;
using System.Diagnostics;

namespace CampingApplication.VisitorApp.ViewModels
{
    public class SpotInfoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private CampingSpotService campingSpotService;


        public int ID {
            get => id;

            set {
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }
        private int id;

        public double Size
        {
            get => size;

            set
            {
                size = value;
                OnPropertyChanged(nameof(Size));
            }
        }
        private double size;

        public double PricePerNight
        {
            get => pricePerNight;

            set
            {
                pricePerNight = value;
                OnPropertyChanged(nameof(PricePerNight));
            }
        }
        private double pricePerNight;

        public SpotInfoViewModel(int ID)
        {
            this.ID = ID;
            campingSpotService = ServiceProvider.Current.Resolve<CampingSpotService>();

        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this.PropertyChanged, new PropertyChangedEventArgs(propertyName));
        }

        public async Task GetCampingSpotInfoAsync()
        {
            try
            {
                var spot = await campingSpotService.GetCampingSpotInfoAsync(ID);
                Size = spot.Size;
                PricePerNight = spot.PricePerNight;

 
                // Use Dispatcher to update UI-bound properties or raise events
                //Application.Current.Dispatcher.Invoke(() =>
                //{
                //    CampingSpots = new ObservableCollection<CampingSpotVisualModel>(campingSpotVisuals);
                //});
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
