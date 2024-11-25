using CampingApplication.Business;
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
            MainViewModel mainViewModel = new();

            MainWindow = new MainWindow(mainViewModel);
            MainWindow.Show();
        }
    }
}
