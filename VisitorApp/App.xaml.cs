using CampingApplication.Business;
using CampingApplication.Business.BookingService;
using CampingApplication.Business.CampingSpotService;
using CampingApplication.VisitorApp.ViewModels;
using DataAccess.Bookings;
using DataAccess;
using System.Configuration;
using System.Data;
using System.Windows;
using CampingApplication.Business.FacilityService;
using CampingApplication.Business.PathService;

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
            DBConnection dbConnection = new();

            ICampingSpotRepository campingSpotRepository = new CampingSpotRepository(dbConnection);
            CampingSpotService campingSpotService = new(campingSpotRepository);
            serviceProvider.RegisterInstance(campingSpotService);

            IFacilityRepository facilityRepository = new FacilityRepository(dbConnection);
            FacilityService facilityService = new(facilityRepository);
            serviceProvider.RegisterInstance(facilityService);

            IBookingRepository bookingRepository = new BookingRepository(dbConnection);
            BookingService bookingService = new(bookingRepository);
            serviceProvider.RegisterInstance(bookingService);

            IPathRepository pathRepository = new PathRepository(dbConnection);
            PathService pathService = new(pathRepository);
            serviceProvider.RegisterInstance(pathService);
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

            IFacilityRepository facilityRepository = new FacilityRepository(dbConnection);
            FacilityService facilityService = new(facilityRepository);
            serviceProvider.RegisterInstance(facilityService);

            IBookingRepository bookingRepository = new BookingRepository(dbConnection);
            BookingService bookingService = new(bookingRepository);
            serviceProvider.RegisterInstance(bookingService);

            IPathRepository pathRepository = new PathRepository(dbConnection);
            PathService pathService = new(pathRepository);
            serviceProvider.RegisterInstance(pathService);
        }

        private void SetWindow(Window window)
        {
            MainWindow = window;
            MainWindow.Show();
        }
    }
}
