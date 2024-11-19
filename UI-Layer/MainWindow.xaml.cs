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

namespace UI_Layer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PlaatsenTonen();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private void PlaatsenTonen()
        {
            
            VoegPlaatsToe(100, 100, true); 
            VoegPlaatsToe(200, 100, false); 
            VoegPlaatsToe(300, 200, true);  
            VoegPlaatsToe(400, 200, false);
            VoegPlaatsToe(500, 300, true);  
            VoegPlaatsToe(600, 300, false); 
        }

        private void VoegPlaatsToe(double x, double y, bool beschikbaar)
        {
            var plaats = new Rectangle
            {
                Width = 50,
                Height = 50,
                Fill = beschikbaar ? Brushes.Green : Brushes.Red
            };
            Canvas.SetLeft(plaats, x);
            Canvas.SetTop(plaats, y);
            ((Canvas)Content).Children.Add(plaats);
        }
    }
}