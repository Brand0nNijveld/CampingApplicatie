using CampingApplication.Business;
using CampingApplication.Business.LoginService;
using CampingApplication.Business.CampingSpotService;
using CampingApplication.EmployeeApp;
using CampingApplication.EmployeeApp.ViewModels;
using System.Configuration;
using System.Data;
using System.Data.Common;
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
            InjectDependencies();
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

            ILoginRepository loginRepository = new LoginRepositoryMock();
            LoginService loginService = new(loginRepository);
            serviceProvider.RegisterInstance(loginService);
        }

        /// <summary>
        /// Inject test dependencies to isolate user controls, or to not interact with database
        /// </summary>

        private void SetWindow(Window window)
        {
            MainWindow = window;
            MainWindow.Show();
        }
    }
}