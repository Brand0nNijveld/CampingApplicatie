using CampingApplication.Business.BookingService;
using CampingApplication.VisitorApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CampingApplication.VisitorApp.Views.Booking
{
    /// <summary>
    /// Interaction logic for BookingView.xaml
    /// </summary>
    public partial class BookingView : UserControl
    {
        private BookingViewModel viewModel;

        public BookingView(int ID, DateTime startDate, DateTime endDate, float pricePerNight)
        {
            InitializeComponent();

            viewModel = new();
            DataContext = viewModel;

            int amountOfNights = BookingService.CalculateAmountOfNights(startDate, endDate);
            float totalPrice = BookingService.CalculateTotalPrice(amountOfNights, pricePerNight);
            Details.SetDetails(ID, startDate, endDate, amountOfNights, pricePerNight, totalPrice);
        }

        public async void Submit_Booking(object sender, RoutedEventArgs e)
        {
            await viewModel.SubmitBooking();
        }
    }
}
