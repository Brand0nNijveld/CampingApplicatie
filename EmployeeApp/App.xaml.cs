using CampingApplication.Business;
using CampingApplication.Business.LoginService;
using CampingApplication.Business.CampingSpotService;
using CampingApplication.EmployeeApp;
using CampingApplication.EmployeeApp.ViewModels;
using DataAccess;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Windows;

namespace CampingApplication.EmployeeApp
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
            ServiceProvider.Current = serviceProvider;  // Assign to the static Current property

            DBConnection dbConnection = new DBConnection();
            ILoginRepository loginRepository = new LoginRepositoryMock();
            LoginService loginService = new(loginRepository);
            serviceProvider.RegisterInstance(loginService);
        }

        /// <summary>
        /// Inject test dependencies to isolate user controls, or to not interact with database
        /// </summary>
        private void InjectDebugDependencies()
        {
            ServiceProvider serviceProvider = new();
            ServiceProvider.Current = serviceProvider;

            DBConnection dbConnection = new();

            ICampingSpotRepository campingSpotRepository = new CampingSpotMockRepository();
            CampingSpotService campingSpotService = new(campingSpotRepository);
            serviceProvider.RegisterInstance(campingSpotService);

            ILoginRepository loginRepository = new LoginRepository(dbConnection);
            LoginService loginService = new(loginRepository);
            serviceProvider.RegisterInstance(loginService);
        }

        private void SetWindow(Window window)
        {
            MainWindow = window;
            MainWindow.Show();
        }
    }
}