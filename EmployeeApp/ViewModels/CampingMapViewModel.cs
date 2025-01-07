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
using CampingApplication.EmployeeApp.Views;

namespace CampingApplication.EmployeeApp.ViewModels
{
    public delegate void AvailabilityHandler(bool available);
    public class CampingMapViewModel : INotifyPropertyChanged
    {

        public List<CampingSpot> CampingSpotData { get; set; } = [];
        public event AvailabilityHandler? AvailabilityChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        private bool _editMode;
        public bool EditMode {
            get => _editMode;
            set
            {
                _editMode = value;
                OnPropertyChanged(nameof(EditMode));
            }
        }

        private ObservableCollection<CampingSpotViewModel> campingSpots = [];
        public ObservableCollection<CampingSpotViewModel> CampingSpots
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

        private CampingSpotService campingSpotService;
        public CampingMapViewModel()
        {
            this.campingSpotService = ServiceProvider.Current.Resolve<CampingSpotService>();
            GetCampingSpotsAsync();
        }

        public async void GetCampingSpotsAsync()
        {
            try
            {
                var spots = await campingSpotService.GetCampingSpotsAsync();
                CampingSpotData = spots;

                var campingSpotVisuals = new CampingSpotViewModel[spots.Count];
                for (int i = 0; i < spots.Count; i++)
                {
                    Debug.WriteLine("Camping spot: " + spots[i].ID);
                    CampingSpot spot = spots[i];
                    campingSpotVisuals[i] = new(spot.ID, spot.PositionX, spot.PositionY);
                }

                // Use Dispatcher to update UI-bound properties or raise events
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CampingSpots = new ObservableCollection<CampingSpotViewModel>(campingSpotVisuals);

                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void AddCampingSpot(int x, int y)
        {
            int newID = 1;
            if (CampingSpots.Count > 0)
            {
                newID = CampingSpots.Max((c) => c.ID) + 1;
            }
            Debug.WriteLine(newID);
            var newSpot = new CampingSpotViewModel(newID, x, y);
            CampingSpots.Add(newSpot);
            OnPropertyChanged(nameof(CampingSpots));
            newSpot.Edited = true;
            EditMode = false;
        }

        public void Save()
        {
            Debug.WriteLine("save functie");
            var unsavedSpots = CampingSpots.Where((c) => c.Edited);
            List<CampingSpot> unsaved = new List<CampingSpot>();

            foreach (var spot in unsavedSpots)
            {
                CampingSpot newSpot = new CampingSpot(spot.ID, spot.PositionX, spot.PositionY);
                unsaved.Add(newSpot);
            }

            campingSpotService.SaveCampingSpots(unsaved);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this.PropertyChanged, new PropertyChangedEventArgs(propertyName));
        }
    }
}
