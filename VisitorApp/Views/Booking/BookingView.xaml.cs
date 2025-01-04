using CampingApplication.Business.BookingService;
using CampingApplication.VisitorApp.Models;
using CampingApplication.VisitorApp.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CampingApplication.VisitorApp.Views.Booking
{
    public delegate void BackButtonHandler();
    /// <summary>
    /// Interaction logic for BookingView.xaml
    /// </summary>
    public partial class BookingView : UserControl
    {
        public event BackButtonHandler? BackButtonClicked;

        public BookingViewModel ViewModel { get; private set; }

        public BookingView(int ID, CampingMapModel mapModel)
        {
            InitializeComponent();

            ViewModel = new(ID, mapModel);
            DataContext = ViewModel;
            Details.SetDetails(ID, mapModel);

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            SystemErrorText.MaxHeight = 0;
            SystemErrorText.Opacity = 0;
            SystemErrorText.MouseLeftButtonUp += SystemErrorText_MouseLeftButtonUp;
        }

        private void SystemErrorText_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ViewModel.SystemError != null)
            {
                ViewModel.SystemError = null;
            }
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.SystemError))
            {
                Debug.WriteLine(e.PropertyName + ": " + ViewModel.SystemError);
                if (!string.IsNullOrEmpty(ViewModel.SystemError))
                {
                    // Show error
                    AnimateError(1, 60);
                }
                else
                {
                    // Hide error
                    AnimateError(0, 0);
                }
            }
        }

        private void AnimateError(double opacity, double MaxHeight)
        {
            var opacityAnimation = new DoubleAnimation
            {
                To = opacity,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                BeginTime = TimeSpan.FromSeconds(0.1),
            };

            var heightAnimation = new DoubleAnimation
            {
                To = MaxHeight,
                Duration = new Duration(TimeSpan.FromSeconds(0.1))
            };

            Storyboard.SetTarget(opacityAnimation, SystemErrorText);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTarget(heightAnimation, SystemErrorText);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath("MaxHeight"));

            var storyBoard = new Storyboard();
            storyBoard.Children.Add(opacityAnimation);
            storyBoard.Children.Add(heightAnimation);
            storyBoard.Begin();
        }

        public async void Submit_Booking(object sender, RoutedEventArgs e)
        {
            await ViewModel.SubmitBooking();
        }

        private void BackButton_Clicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BackButtonClicked?.Invoke();
        }
    }
}
