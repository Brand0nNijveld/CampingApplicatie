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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CampingApplication.VisitorApp.Views
{
    /// <summary>
    /// Interaction logic for BookingView.xaml
    /// </summary>
    public partial class BookingView : UserControl
    {
        private BookingViewModel viewModel;

        public BookingView()
        {
            InitializeComponent();

            viewModel = new();
            DataContext = viewModel;
        }

        public async void Submit_Booking(object sender, RoutedEventArgs e)
        {
            await viewModel.SubmitBooking();
        }
    }
}
