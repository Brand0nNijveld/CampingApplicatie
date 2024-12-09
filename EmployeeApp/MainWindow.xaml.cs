using CampingApplication.Business;
using CampingApplication.EmployeeApp.ViewModels;
using CampingApplication.EmployeeApp.Views.Login;
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

namespace CampingApplication.EmployeeApp
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
            var loginView = new LoginView();
            this.Content = loginView;

            //CampingMapUserControl.DataContext = viewModel.CampingMapViewModel;
            //CampingMapUserControl.SetViewModel(viewModel.CampingMapViewModel);

            ActionPanel.DataContext = viewModel.ActionPanelViewModel;
            ActionPanel.SetViewModel(viewModel.ActionPanelViewModel);

            //DateRangePicker.mainViewModel = viewModel;
        }
    }
}