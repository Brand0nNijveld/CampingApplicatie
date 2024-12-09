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

        public event ButtonClickHandler? CloseButton_Clicked;

        public SpotInfoViewModel SpotInfoViewModel { get; set; }
        public SpotInfo(int ID)
        {
            InitializeComponent();
            SpotInfoViewModel = new SpotInfoViewModel(ID);
            DataContext = SpotInfoViewModel;
            this.Loaded += SpotInfo_Loaded;
          
            Visibility = Visibility.Visible; 
        }

        private async void SpotInfo_Loaded(object sender, RoutedEventArgs e)
        {
            await SpotInfoViewModel.GetCampingSpotInfoAsync();
           
        }

        // Close the control when the Close button is clicked
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseButton_Clicked?.Invoke();    // Hide the control
        }
    }
}
