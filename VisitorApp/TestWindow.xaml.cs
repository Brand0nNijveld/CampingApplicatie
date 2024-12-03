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
using CampingApplication.VisitorApp.Views.Booking;
using System.Windows.Shapes;

namespace CampingApplication.VisitorApp
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        /// <summary>
        /// Use this class to test individual windows or components.
        /// </summary>
        public TestWindow()
        {
            InitializeComponent();

            BookingStepsPanel view = new(1, DateTime.Now, DateTime.Now.AddDays(5));
            setWindow(view);
        }

        private void setWindow(UIElement element)
        {
            ContentWindow.Children.Add(element);
        }
    }
}
