using CampingApplication.Business;
using CampingApplication.Business.CampingSpotService;
using CampingApplication.VisitorApp.ViewModels;
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
            ServiceProvider serviceProvider = new();
            
            DBconnection dbConnection = new DBconnection();
            ICampingSpotRepository campingSpotRepository = new CampingSpotRepository(dbConnection);
            CampingSpotService campingSpotService = new(campingSpotRepository);

            serviceProvider.RegisterInstance<CampingSpotService>(campingSpotService);

            MainViewModel mainViewModel = new();

            MainWindow = new MainWindow(mainViewModel);
            MainWindow.Show();
        }
    }
}
