using CampingApplication.VisitorApp.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace CampingApplication.VisitorApp.Views.Information
{
    /// <summary>
    /// Interaction logic for SpotInfo.xaml
    /// </summary>
    public partial class SpotInfo : UserControl
    {
        // Declare the event handler
        public event Action? CloseButton_Clicked;

        public SpotInfoViewModel SpotInfoViewModel { get; set; }

        // Constructor to initialize the SpotInfo UserControl
        public SpotInfo(int ID, int numberOfNights)
        {
            InitializeComponent();

            SpotInfoViewModel = new SpotInfoViewModel(ID, numberOfNights);

            DataContext = SpotInfoViewModel;

            this.Loaded += SpotInfo_Loaded;

            Visibility = Visibility.Visible;
        }

        // Event handler for the Loaded event
        private async void SpotInfo_Loaded(object sender, RoutedEventArgs e)
        {
            await SpotInfoViewModel.LoadCampingSpotInfoAsync();
        }

        // Close button click event handler
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            //Visibility = Visibility.Collapsed;
            // Trigger the CloseButton_Clicked event
            CloseButton_Clicked?.Invoke();
        }
    }
}
