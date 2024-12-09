using CampingApplication.Business;
using CampingApplication.Business.BookingService;
using CampingApplication.Business.CampingSpotService;
using CampingApplication.VisitorApp.ViewModels;
using DataAccess.Bookings;
using DataAccess;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CampingApplication.VisitorApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        /// <summary>
        /// Creates dependencies, injects them and initializes main window.
        /// </summary>
        public App()
        {
#if DEBUG
            InjectDebugDependencies();
            MainWindow testWindow = new();
            SetWindow(testWindow);
#else
            InjectDependencies();
            MainWindow window = new();
            SetWindow(window);
#endif
        }

        /// <summary>
        /// Inject dependencies for production app
        /// </summary>
        private void InjectDependencies()
        {
            ServiceProvider serviceProvider = new();

            DBConnection dbConnection = new DBConnection();
            ICampingSpotRepository campingSpotRepository = new CampingSpotRepository(dbConnection);
            CampingSpotService campingSpotService = new(campingSpotRepository);
            serviceProvider.RegisterInstance(campingSpotService);

            IBookingRepository bookingRepository = new BookingRepositoryMock();
            BookingService bookingService = new(bookingRepository);
            serviceProvider.RegisterInstance(bookingService);
        }

        /// <summary>
        /// Inject test dependencies to isolate user controls, or to not interact with database
        /// </summary>
        private void InjectDebugDependencies()
        {
            ServiceProvider serviceProvider = new();
            DBConnection dbConnection = new();

            ICampingSpotRepository campingSpotRepository = new CampingSpotRepository(dbConnection);
            CampingSpotService campingSpotService = new(campingSpotRepository);
            serviceProvider.RegisterInstance(campingSpotService);

            IBookingRepository bookingRepository = new BookingRepository(dbConnection);
            BookingService bookingService = new(bookingRepository);
            serviceProvider.RegisterInstance(bookingService);
        }

        private void SetWindow(Window window)
        {
            MainWindow = window;
            MainWindow.Show();
        }
    }
}
