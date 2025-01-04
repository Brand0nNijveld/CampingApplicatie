using CampingApplication.Business.PathService;
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

        public PathView? PathView { get; set; }

        public CampingMap()
        {
            InitializeComponent();

            // Load the image
            var bitmap = new BitmapImage(new Uri("Resources/TestMap3.png", UriKind.Relative));
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
                CampingSpotViewModel campingSpotViewModel = new(viewModel.Model, campingSpot.Value);
                var campingSpotView = new CampingSpotView(campingSpotViewModel);

                Canvas.SetLeft(campingSpotView, campingSpotViewModel.PositionX);
                Canvas.SetTop(campingSpotView, campingSpotViewModel.PositionY);
                Canvas.SetZIndex(campingSpotView, 2);
                CampingCanvas.Children.Add(campingSpotView);
            }

            // Draw facilities
            foreach (var facility in viewModel.Facilities)
            {
                FacilityViewModel facilityViewModel = new(facility);
                var facilityView = new FacilityView(facilityViewModel);
                double width = facilityView.Width;
                double height = facilityView.Height;

                // Set it to center so route goes to the center (inconsistent with camping spots right now)
                Canvas.SetLeft(facilityView, facilityViewModel.PositionX - width / 2);
                Canvas.SetTop(facilityView, facilityViewModel.PositionY - height / 2);
                Canvas.SetZIndex(facilityView, 2);

                CampingCanvas.Children.Add(facilityView);
            }

            PathView?.DrawMainPath();
            CampingCanvas.UpdateLayout();
        }

        private void CampingCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(CampingCanvas);
            var (xCoordinate, yCoordinate) = MapConversionHelper.PixelsToMeters(pos.X, pos.Y, CampingMapViewModel.PIXELS_PER_METER);
            Debug.WriteLine("Real world mouse position: (x)" + xCoordinate + ", (y)" + yCoordinate);

        }
    }
}
