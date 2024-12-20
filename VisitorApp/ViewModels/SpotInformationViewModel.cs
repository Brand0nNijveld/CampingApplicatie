using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.VisitorApp.ViewModels
{
    public class SpotInformationViewModel : INotifyPropertyChanged
    {
        private PathViewModel pathViewModel;
        private CampingMapViewModel mapViewModel;
        private bool open;
        public bool Open
        {
            get => open;
            set
            {
                open = value;
                OnPropertyChanged(nameof(Open));
            }
        }

        private string title = "";
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public SpotInformationViewModel(CampingMapViewModel mapViewModel, PathViewModel pathViewModel)
        {
            this.pathViewModel = pathViewModel;
            this.mapViewModel = mapViewModel;
            mapViewModel.SelectedCampingSpotChanged += MapViewModel_SelectedCampingSpotChanged;
            pathViewModel.PropertyChanged += PathViewModel_PropertyChanged;
        }

        private void PathViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(pathViewModel.Routes))
            {
                foreach (var route in pathViewModel.Routes)
                {
                    Debug.WriteLine("Route: " + route.TotalDistance);
                }
            }
        }

        private async void MapViewModel_SelectedCampingSpotChanged(CampingSpotViewModel? campingSpot)
        {
            Open = campingSpot != null;

            if (campingSpot != null)
            {
                Title = $"Campingplek #{campingSpot.ID}";
                await Task.Delay(1000);
                await pathViewModel.CreateRoutes(campingSpot);
            }
        }

        public void Close()
        {
            mapViewModel.SelectCampingSpot(null);
            pathViewModel.Routes = [];
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
