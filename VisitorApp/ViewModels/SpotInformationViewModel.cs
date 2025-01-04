using CampingApplication.Business.PathService;
using CampingApplication.VisitorApp.Models;
using CampingApplication.VisitorApp.Views.Booking;
using CampingApplication.VisitorApp.Views.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.VisitorApp.ViewModels
{
    public delegate void StartBookingProcessHandler();
    public class SpotInformationViewModel : BaseViewModel
    {
        public event StartBookingProcessHandler? BookingProcessStarted;

        private SpotInformationModel? currentSpotInformation;

        private CampingMapModel campingMapModel;
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

        private string title = "Campingplek ?";
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private ButtonState bookButtonState;
        public ButtonState BookButtonState
        {
            get => bookButtonState;
            set
            {
                if (bookButtonState != value)
                {
                    bookButtonState = value;
                    OnPropertyChanged(nameof(BookButtonState));
                }
            }
        }


        private ObservableCollection<FacilityRouteModel> facilityRoutes = [];
        public ObservableCollection<FacilityRouteModel> FacilityRoutes
        {
            get => facilityRoutes;
            set
            {
                facilityRoutes = value;
                OnPropertyChanged(nameof(FacilityRoutes));
            }
        }

        public SpotInformationViewModel(CampingMapModel campingMapModel)
        {
            this.campingMapModel = campingMapModel;
            campingMapModel.PropertyChanged += CampingMapModel_PropertyChanged;
        }

        private void CampingMapModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(campingMapModel.SelectedCampingSpot))
            {
                OnSelectedCampingSpotChanged(campingMapModel.SelectedCampingSpot);
            }
            else if (e.PropertyName == nameof(campingMapModel.AvailableCampingSpots))
            {
                SetBookButtonState();
            }
        }

        private void SetBookButtonState()
        {
            if (currentSpotInformation != null)
            {
                if (campingMapModel.AvailableCampingSpots != null)
                {
                    bool available = campingMapModel.AvailableCampingSpots.ContainsKey(currentSpotInformation.CampingSpot.ID);
                    BookButtonState = available ? ButtonState.Active : ButtonState.Inactive;
                }
                else
                {
                    BookButtonState = ButtonState.Inactive;
                }
            }
        }

        private void OnSelectedCampingSpotChanged(SpotInformationModel? campingSpot)
        {
            Open = campingSpot != null;
            currentSpotInformation = campingSpot;

            if (campingSpot != null)
            {
                Title = $"Campingplek #{campingSpot.CampingSpot.ID}";
                SetBookButtonState();

                campingSpot.PropertyChanged += SelectedSpot_PropertyChanged;
                ClearAsyncData();
            }
        }
        
        public void StartBookingProcess()
        {
            BookingProcessStarted?.Invoke();
        }

        private void SelectedSpot_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is SpotInformationModel model)
            {
                if (e.PropertyName == nameof(model.Routes))
                {
                    FacilityRoutes = new(model.Routes);
                }
            }
        }

        public void HoverFacilityRoute(FacilityRouteModel facilityRoute)
        {
            if (campingMapModel.SelectedCampingSpot != null)
                campingMapModel.SelectedCampingSpot.HoveredFacilityRoute = facilityRoute;
        }

        public void StopHoverFacilityRoute()
        {
            if (campingMapModel.SelectedCampingSpot != null)
                campingMapModel.SelectedCampingSpot.HoveredFacilityRoute = null;
        }

        public void Close()
        {
            campingMapModel.SelectedCampingSpot = null;
        }

        public void ClearAsyncData()
        {
            FacilityRoutes = [];
        }
    }
}
