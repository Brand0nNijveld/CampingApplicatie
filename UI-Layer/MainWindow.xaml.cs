using CampingApplication.Business;
using CampingApplication.VisitorApp.ViewModels;
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
        public MainWindow()
        {
            InitializeComponent();

            CampingSpot[] campingSpots =
                [
                    new(0, 188, 32, []),
                ];

            CampingMapViewModel campingMapViewModel = new("../test1.png", campingSpots);
            CampingMapUserControl.DataContext = campingMapViewModel;
            CampingMapUserControl.SetViewModel(campingMapViewModel);
        }
    }
}