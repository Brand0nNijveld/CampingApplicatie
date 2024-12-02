using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CampingApplication.VisitorApp.Views.Booking
{
    public delegate void BookButtonHandler();

    /// <summary>
    /// Interaction logic for Temp_CampingSpotInfo.xaml
    /// </summary>
    public partial class Temp_CampingSpotInfo : UserControl
    {
        public event BookButtonHandler? BookButtonClicked;

        public Temp_CampingSpotInfo()
        {
            InitializeComponent();
        }

        private void BookButton_Clicked(object sender, RoutedEventArgs e)
        {
            BookButtonClicked?.Invoke();
        }
    }
}
