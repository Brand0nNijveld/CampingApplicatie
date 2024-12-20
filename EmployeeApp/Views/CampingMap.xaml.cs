﻿using CampingApplication.EmployeeApp;
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

            foreach (var spot in viewModel.CampingSpots)
            {
                var spotVisual = new Rectangle
                {
                    Width = width,
                    Height = height,
                    RadiusX = 5,
                    RadiusY = 5,
                    Fill = spot.Available ? Brushes.Yellow : Brushes.Transparent,
                    Cursor = Cursors.Hand,
                    IsHitTestVisible = true,
                };

                Debug.WriteLine("Drawing Camping Spot: " + spot.ID);

                //spotVisual.MouseLeftButtonUp += (e, s) =>
                //{
                //    Debug.WriteLine("Clicked cmaping spot");
                //    viewModel?.ShowBookScreen(spot.ID);
                //};

                spotVisual.MouseLeftButtonUp += (s, e) =>
                {
                    Debug.WriteLine("Clicked camping spot");

                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    //if (mainWindow != null)
                    //{
                    //    mainWindow.SpotInfo.Visibility = Visibility.Visible;
                    //}
                };

                // Add the visual to the CampingCanvas
                CampingCanvas.Children.Add(spotVisual);



                Canvas.SetLeft(spotVisual, spot.PositionX);
                Canvas.SetTop(spotVisual, spot.PositionY);

                var spotID = new TextBlock
                {
                    Text = spot.ID.ToString(),
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Colors.Black),
                    IsHitTestVisible = false,
                };

                spotID.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                var textSize = spotID.DesiredSize;

                double textPosX = spot.PositionX + (width / 2) - (textSize.Width / 2);
                double textPosY = spot.PositionY + (height / 2) - (textSize.Height / 2);

                Canvas.SetLeft(spotID, textPosX);
                Canvas.SetTop(spotID, textPosY);

                // Handle click event 
                //spotVisual.MouseLeftButtonUp += (s, e) =>
                //{
                //    // When a camping spot is clicked, show the white rectangle and update information
                //    HighlightRectangle.Visibility = Visibility.Visible;

                //    // Update the camping spot info
                //    CampingSpotToiletDistance.Text = "Afstand tot Toiletgebouw: 15m";
                //    CampingSpotLakeDistance.Text = "Afstand tot Meer: 25m";
                //    CampingSpotSize.Text = "Grootte van plaats: 30m²";
                //    CampingSpotReceptionDistance.Text = "Afstand tot Receptie: 10m";
                //    CampingSpotInfo.Text = "Plaats 1 Informatie";
                //    CampingSpotType.Text = "Plaatstype: Tent";
                //    CampingSpotPrice.Text = "Prijs per nacht: €50";
                //    CampingSpotAvailability.Text = "Beschikbaar vanaf xx/xx/xxxx";
                //};

                //CampingCanvas.Children.Add(spotVisual);
                CampingCanvas.Children.Add(spotID);
            }
        }

        private void SpotVisual_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }
    }
}