using CampingApplication.VisitorApp.Models;
using CampingApplication.VisitorApp.ViewModels;
using CampingApplication.VisitorApp.Views.Components;
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

namespace CampingApplication.VisitorApp.Views.Information
{
    /// <summary>
    /// Interaction logic for SpotInformationView.xaml
    /// </summary>
    public partial class SpotInformationView : UserControl
    {
        public SpotInformationViewModel? viewModel;

        public SpotInformationView()
        {
            InitializeComponent();
            VisualStateManager.GoToState(this, "Closed", false);
        }

        public void SetViewModel(SpotInformationViewModel viewModel)
        {
            this.viewModel = viewModel;
            DataContext = this.viewModel;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (viewModel == null)
                return;

            if (e.PropertyName == nameof(viewModel.Open))
            {
                if (viewModel.Open)
                {
                    VisualStateManager.GoToState(this, "Open", true);
                }
                else
                {
                    VisualStateManager.GoToState(this, "Closed", true);
                }
            }
        }

        private void CloseButton_Clicked(object sender, MouseButtonEventArgs e)
        {
            viewModel?.Close();
        }

        private void CloseAnimation_Completed(object sender, EventArgs e)
        {
            viewModel?.ClearAsyncData();
        }

        private void FacilityRoute_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border && border.Child is UIElement child)
            {
                // Apply a ScaleTransform if not already applied
                if (child.RenderTransform is not ScaleTransform scaleTransform)
                {
                    scaleTransform = new ScaleTransform(1, 1);
                    child.RenderTransform = scaleTransform;
                    child.RenderTransformOrigin = new Point(0.5, 0.5); // Scale from the center
                }

                border.Background = new SolidColorBrush(Colors.White);
                // Animate the scaling up
                AnimateScale(scaleTransform, 0.95); // Scale up to 110%

                if (border.DataContext is FacilityRouteModel facilityRoute)
                {
                    viewModel?.HoverFacilityRoute(facilityRoute);
                }
            }
        }

        private void FacilityRoute_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border && border.Child is UIElement child)
            {
                border.Background = new SolidColorBrush(Colors.Transparent);
                if (child.RenderTransform is ScaleTransform scaleTransform)
                {
                    // Animate the scaling back to normal
                    AnimateScale(scaleTransform, 1.0); // Scale back to 100%
                }
            }

            viewModel?.StopHoverFacilityRoute();
        }

        private void AnimateScale(ScaleTransform scaleTransform, double targetScale)
        {
            var duration = TimeSpan.FromMilliseconds(200);

            // Animate ScaleX
            var scaleXAnimation = new DoubleAnimation
            {
                To = targetScale,
                Duration = new Duration(duration),
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
            };

            // Animate ScaleY
            var scaleYAnimation = new DoubleAnimation
            {
                To = targetScale,
                Duration = new Duration(duration),
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
            };

            // Apply animations to the ScaleTransform
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleYAnimation);
        }

        private void BookButton_Clicked(object sender, RoutedEventArgs e)
        {
            viewModel?.StartBookingProcess();
        }
    }
}
