using CampingApplication.Business;
using CampingApplication.Business.CampingSpotService;
using DataAccess;
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
using System.Windows.Shapes;

namespace CampingApplication.VisitorApp.Views
{
    /// <summary>
    /// Interaction logic for TempEmployeeButton.xaml
    /// </summary>
    public partial class TempEmployeeButton : UserControl
    {
        public CampingSpotService CampingSpotService { get; private set; }

        private bool waitingForClick = false;

        private int Xcordinate;
        private int Ycordinate;
        private int SpotNr;


        public TempEmployeeButton()
        {
            InitializeComponent();
            CampingSpotService = ServiceProvider.Current.Resolve<CampingSpotService>();
        }

        private void Plaats_Click(object sender, RoutedEventArgs e)
        {
            AddNotification.Text = "klik waar je een nieuwe plaats wil hebben";

            waitingForClick = true;
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (waitingForClick)
            {
                var position = e.GetPosition(AddSpot);
                Xcordinate = (int)position.X;
                Ycordinate = (int)position.Y;
                SpotNr = int.Parse(SpotNumber.Text);


                AddNotification.Text = $"Je hebt geklikt op: X={position.X}, Y={position.Y}";


                Button newButton = new Button();
                {
                    Width = 20;
                    Height = 20;
                };
                Canvas.SetLeft(newButton, position.X - 10);
                Canvas.SetTop(newButton, position.Y - 10);
                AddSpot.Children.Add(newButton);

                waitingForClick = false;
            }
        }

        private void Opslaan_Click(object sender, RoutedEventArgs e)
        {
            string returntext = CampingSpotService.AddCampingSpot(SpotNr, Xcordinate, Ycordinate);
            AddNotification.Text = returntext;
        }

        private void SpotNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            {
                TextBox textBox = sender as TextBox;
                if (!int.TryParse(textBox.Text, out _))
                {
                    textBox.Text = "";
                }
            }
        }
    }
}
