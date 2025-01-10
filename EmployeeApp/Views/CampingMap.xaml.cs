using CampingApplication.EmployeeApp;
using CampingApplication.EmployeeApp.ViewModels;
using CampingApplication.EmployeeApp.Views.Information;
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

namespace CampingApplication.EmployeeApp.Views
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
        }

        public void SetViewModel(CampingMapViewModel viewModel)
        {
            this.viewModel = viewModel;
            DataContext = viewModel;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.AvailabilityChanged += OnAvailabilityChanged;

            UpdateCanvas();
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
            if (e.PropertyName == nameof(CampingMapViewModel.CampingSpots))
            {
                UpdateCanvas();
            }
        }

        //private void CloseButton_Click(object sender, RoutedEventArgs e)
        //{
        //    HighlightRectangle.Visibility = Visibility.Collapsed;
        //}

        private void BoekenButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Boekingsproces gestart!", "Boeken", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void UpdateCanvas()
        {
            if (viewModel == null)
            {
                return;
            }

            CampingCanvas.Children.Clear();

            int width = 81;
            int height = 60;

            foreach (var campingSpot in viewModel.CampingSpots)
            {
                var campingSpotView = new CampingSpotView(campingSpot);

                Canvas.SetLeft(campingSpotView, campingSpot.PositionX);
                Canvas.SetTop(campingSpotView, campingSpot.PositionY);
                CampingCanvas.Children.Add(campingSpotView);


            }
            Debug.WriteLine("Tekenen spot");
        }

        private void CampingCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (viewModel.EditMode)
            {
                var position = e.GetPosition((Canvas)sender);
                int Xcordinate = (int)position.X;
                int Ycordinate = (int)position.Y;

                viewModel.AddCampingSpot(Xcordinate, Ycordinate);


                Debug.WriteLine(Xcordinate + ", " + Ycordinate);
            }
        }
    }
}