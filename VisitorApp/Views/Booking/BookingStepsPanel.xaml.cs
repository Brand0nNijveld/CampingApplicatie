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
    /// <summary>
    /// Interaction logic for BookingStepsPanel.xaml
    /// </summary>
    public partial class BookingStepsPanel : UserControl
    {
        private int currentStep;
        public int CurrentStep
        {
            get => currentStep;
            set
            {
                currentStep = value;
                ContentPanel.Content = steps[currentStep];
            }
        }

        private List<UserControl> steps;

        public BookingStepsPanel(int ID, DateTime startDate, DateTime endDate)
        {
            InitializeComponent();

            Temp_CampingSpotInfo infoPanel = new();
            infoPanel.BookButtonClicked += () => CurrentStep = 1;

            BookingView bookingView = new(ID, startDate, endDate, 60);
            bookingView.BackButtonClicked += () => CurrentStep = 0;
            bookingView.ViewModel.BookingSuccessful += () => CurrentStep = 2;

            BookingSuccessView bookingSuccessView = new();

            steps =
                [
                    infoPanel,
                    bookingView,
                    bookingSuccessView
                ];

            ContentPanel.Content = steps[currentStep];
        }
    }
}
