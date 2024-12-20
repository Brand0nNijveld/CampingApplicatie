using CampingApplication.Business;
using CampingApplication.VisitorApp.ViewModels;
using CampingApplication.VisitorApp.Views.Booking;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CampingApplication.VisitorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new();
            DataContext = viewModel;

            CampingMapUserControl.DataContext = viewModel.CampingMapViewModel;
            CampingMapUserControl.SetViewModel(viewModel.CampingMapViewModel);

            // Create path view model to get path data and create the path view for the map
            CampingMapUserControl.PathView = new(CampingMapUserControl.CampingCanvas, viewModel.PathViewModel);

            SpotInformation.SetViewModel(viewModel.SpotInformationViewModel);

            ActionPanel.DataContext = viewModel.ActionPanelViewModel;
            ActionPanel.SetViewModel(viewModel.ActionPanelViewModel);

            DateRangePicker.mainViewModel = viewModel;
        }
    }
}