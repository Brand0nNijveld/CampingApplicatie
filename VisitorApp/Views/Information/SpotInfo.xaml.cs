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

namespace CampingApplication.VisitorApp.Views.Information
{
    /// <summary>
    /// Interaction logic for SpotInfo.xaml
    /// </summary>
    public partial class SpotInfo : UserControl
    {
        public SpotInfo()
        {
            InitializeComponent();
            Visibility = Visibility.Collapsed; // Initially hidden
        }

        // Close the control when the Close button is clicked
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;      // Hide the control
        }
    }
}
