using CampingApplication.VisitorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CampingApplication.VisitorApp.Views
{
    /// <summary>
    /// Interaction logic for CampingMap.xaml
    /// </summary>
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
                    Cursor = Cursors.Hand
                };

                Canvas.SetLeft(spotVisual, spot.PositionX);
                Canvas.SetTop(spotVisual, spot.PositionY);

                var spotID = new TextBlock
                {
                    Text = spot.ID.ToString(),
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Colors.Black)
                };

                spotID.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                var textSize = spotID.DesiredSize;

                double textPosX = spot.PositionX + (width / 2) - (textSize.Width / 2);
                double textPosY = spot.PositionY + (height / 2) - (textSize.Height / 2);

                Canvas.SetLeft(spotID, textPosX);
                Canvas.SetTop(spotID, textPosY);

                CampingCanvas.Children.Add(spotVisual);
                CampingCanvas.Children.Add(spotID);
            }
        }
    }
}
