using CampingApplication.Business;
using CampingApplication.Business.CampingSpotService;
using CampingApplication.VisitorApp.ViewModels;
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
            TestWindow testWindow = new();
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

            ICampingSpotRepository campingSpotRepository = new CampingSpotMockRepository();
            CampingSpotService campingSpotService = new(campingSpotRepository);

            serviceProvider.RegisterInstance<CampingSpotService>(campingSpotService);
        }

        /// <summary>
        /// Inject test dependencies to isolate user controls, or to not interact with database
        /// </summary>
        private void InjectDebugDependencies()
        {
            ServiceProvider serviceProvider = new();

            ICampingSpotRepository campingSpotRepository = new CampingSpotMockRepository();
            CampingSpotService campingSpotService = new(campingSpotRepository);

            serviceProvider.RegisterInstance<CampingSpotService>(campingSpotService);
        }

        private void SetWindow(Window window)
        {
            MainWindow = window;
            MainWindow.Show();
        }
    }
}
