using CampingApplication.Business.PathFinding;
using CampingApplication.Client.Shared.Helpers;
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
using System.Windows.Media.Imaging;
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

            // Load the image
            var bitmap = new BitmapImage(new Uri("Resources/TestMap2.png", UriKind.Relative));
            MapImage.ImageSource = bitmap;

            // Set canvas size to match the image dimensions for correct scale conversion
            CampingCanvas.Width = bitmap.PixelWidth;
            CampingCanvas.Height = bitmap.PixelHeight;

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
            viewModel.PropertyChanged += ViewModel_PropertyChanged;

            viewModel.SetWidthAndHeight(CampingCanvas.Width, CampingCanvas.Height);
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (viewModel == null)
            {
                return;
            }

            if (e.PropertyName == nameof(viewModel.MapWidthInMeters))
            {
                MapWidth.Text = FormatMeters(viewModel.MapWidthInMeters);
            }
            else if (e.PropertyName == nameof(viewModel.MapHeightInMeters))
            {
                MapHeight.Text = FormatMeters(viewModel.MapHeightInMeters);
            }
        }

        private string FormatMeters(double meters)
        {
            return Math.Floor(meters) + " meter";
        }

        private void ViewModel_MapLoadError()
        {
            Debug.WriteLine("FAILED LOADING MAP");
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
                var facilityView = new FacilityView(facility);

                Canvas.SetLeft(facilityView, facility.PositionX);
                Canvas.SetTop(facilityView, facility.PositionY);
                Debug.WriteLine("Setting facility position: " + facility.PositionX + ", " + facility.PositionY);
                CampingCanvas.Children.Add(facilityView);
            }

            PathView pathView = new(CampingCanvas);
            pathView.DrawMainPath();
        }

        private void CampingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(CampingCanvas);
            double xCoordinate = MapConversionHelper.PixelsToMeters(pos.X, CampingMapViewModel.PIXELS_PER_METER);
            double yCoordinate = MapConversionHelper.PixelsToMeters(pos.Y, CampingMapViewModel.PIXELS_PER_METER);
            Debug.WriteLine("Real world mouse position: (x)" + xCoordinate + ", (y)" + yCoordinate);
        }
    }
}
