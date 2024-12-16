using CampingApplication.VisitorApp.ViewModels;
using CampingApplication.VisitorApp.Views.Booking;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CampingApplication.VisitorApp.Views.Information
{
    /// <summary>
    /// Interaction logic for SpotInfo.xaml
    /// </summary>
    public partial class SpotInfo : UserControl
    {
        // Event to handle the close button click
        public event ButtonClickHandler? CloseButton_Clicked;

        // ViewModel property for data binding
        public SpotInfoViewModel SpotInfoViewModel { get; set; }

        // Constructor
        public SpotInfo(int ID)
        {
            InitializeComponent();

            // Initialize the ViewModel and pass the ID
            SpotInfoViewModel = new SpotInfoViewModel(ID);

            // Set the DataContext for data binding
            DataContext = SpotInfoViewModel;

            // Subscribe to Loaded event to load data when the control is loaded
            this.Loaded += SpotInfo_Loaded;

            // Make sure the control is visible
            Visibility = Visibility.Visible;
        }

        // Async method called when the UserControl is loaded
        private async void SpotInfo_Loaded(object sender, RoutedEventArgs e)
        {
            // Call the async method to fetch the camping spot data
            await SpotInfoViewModel.LoadCampingSpotInfoAsync();
        }

        // Close button click handler
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseButton_Clicked?.Invoke();  // Invoke the event when the close button is clicked
        }
    }
}
