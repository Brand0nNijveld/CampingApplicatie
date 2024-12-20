using CampingApplication.VisitorApp.ViewModels;
using CampingApplication.VisitorApp.Views.Information;
using System;
using System.Diagnostics;
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

                spotVisual.MouseLeftButtonUp += (e, s) =>
                {
                    Debug.WriteLine("Clicked camping spot");

                    // Haal de MainWindow instantie op
                    var mainWindow = Application.Current.MainWindow as MainWindow;

                    if (mainWindow != null)
                    {
                        // Haal de begin- en einddatum vanuit de MainWindow
                        DateTime? beginDate = mainWindow.GetBeginDate();
                        DateTime? endDate = mainWindow.GetEndDate();

                        // Controleer of beide datums zijn geselecteerd
                        if (beginDate.HasValue && endDate.HasValue)
                        {
                            // Bereken het aantal nachten
                            viewModel?.UpdateNumberOfNights(beginDate.Value, endDate.Value);

                            // Roep de ShowBookScreen aan met de datums
                            viewModel?.ShowBookScreen(spot.ID, beginDate.Value, endDate.Value);
                        }
                        else
                        {
                            // Geef een foutmelding als niet beide datums zijn geselecteerd
                            MessageBox.Show("Selecteer beide datums.");
                        }
                    }
                };

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

                CampingCanvas.Children.Add(spotID);
            }
        }

        private void SpotVisual_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
