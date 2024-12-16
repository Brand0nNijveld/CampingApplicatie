using CampingApplication.VisitorApp.ViewModels;
using CampingApplication.VisitorApp.Views.Information;
using CampingApplication.VisitorApp.Views.Map;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CampingApplication.VisitorApp.Views
{
    public partial class CampingMap : UserControl
    {
        private CampingMapViewModel? viewModel;
        DoubleAnimation fadeInAnimation;
        DoubleAnimation fadeOutAnimation;

        public CampingMap()
        {
            InitializeComponent();

            fadeInAnimation = new DoubleAnimation
            {
                From = 0,   // Start from fully opaque
                To = 1,     // End at fully transparent
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };

            fadeOutAnimation = new DoubleAnimation
            {
                From = 1,   // Start from fully opaque
                To = 0,     // End at fully transparent
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };

            this.Loaded += UI_Loaded;
        }

        private async void UI_Loaded(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
                await viewModel.GetMapDataAsync();
        }

        public void SetViewModel(CampingMapViewModel viewModel)
        {
            this.viewModel = viewModel;
            viewModel.AvailabilityChanged += OnAvailabilityChanged;

            viewModel.MapLoaded += ViewModel_MapLoaded;
            viewModel.MapLoadError += ViewModel_MapLoadError;
        }

        private void ViewModel_MapLoadError()
        {
            throw new NotImplementedException();
        }

        private void ViewModel_MapLoaded()
        {
            DrawMap();
        }

        private void OnAvailabilityChanged(bool available)
        {
            if (available)
            {
                if (NoneAvailableInfo.Opacity == 1)
                {
                    NoneAvailableInfo.BeginAnimation(OpacityProperty, fadeOutAnimation);
                }
            }
            else
            {
                if (NoneAvailableInfo.Opacity == 0)
                {
                    NoneAvailableInfo.IsEnabled = true;
                    NoneAvailableInfo.BeginAnimation(OpacityProperty, fadeInAnimation);
                }
            }
        }

        public void DrawMap()
        {
            if (viewModel == null)
            {
                return;
            }

            CampingCanvas.Children.Clear();

            // Draw camping spots
            foreach (var campingSpot in viewModel.CampingSpots)
            {
                var campingSpotView = new CampingSpotView(campingSpot);

                Canvas.SetLeft(campingSpotView, campingSpot.PositionX);
                Canvas.SetTop(campingSpotView, campingSpot.PositionY);
                CampingCanvas.Children.Add(campingSpotView);
            }

            // Draw facilities
            foreach (var facility in viewModel.Facilities)
            {
                var facilityView = new FacilityView();

                Canvas.SetLeft(facilityView, facility.PositionX);
                Canvas.SetTop(facilityView, facility.PositionY);
                CampingCanvas.Children.Add(facilityView);
            }
        }
    }
}
