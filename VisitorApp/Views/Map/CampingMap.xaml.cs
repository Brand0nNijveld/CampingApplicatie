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

            this.Loaded += CampingMap_Loaded;
        }

        private async void CampingMap_Loaded(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
                await viewModel.GetCampingSpotsAsync();
        }

        public void SetViewModel(CampingMapViewModel viewModel)
        {
            this.viewModel = viewModel;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.AvailabilityChanged += OnAvailabilityChanged;
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

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.CampingSpots))
            {
                Debug.WriteLine("Drawing map");
                DrawMap();
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
            foreach (var spot in viewModel.CampingSpots)
            {
                var campingSpot = new CampingSpotView(spot);

                Canvas.SetLeft(campingSpot, spot.PositionX);
                Canvas.SetTop(campingSpot, spot.PositionY);
                CampingCanvas.Children.Add(campingSpot);
            }

            // Draw facilities

        }
    }
}
