using CampingApplication.Business;
using CampingApplication.Business.CampingSpotService;
using CampingApplication.EmployeeApp.ViewModels;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace CampingApplication.EmployeeApp.Views.AddCampingSpot
{
    /// <summary>
    /// Interaction logic for AddCampingSpot.xaml
    /// </summary>
    public partial class AddCampingSpot : UserControl
    {
        public CampingSpotService CampingSpotService { get; private set; }
        public CampingMapViewModel MapViewModel { get; set; }


        private bool waitingForClick = false;

        private int Xcordinate;
        private int Ycordinate;
        private int SpotNr;


        public AddCampingSpot()
        {
            InitializeComponent();
            CampingSpotService = ServiceProvider.Current.Resolve<CampingSpotService>();
        }

        public void SetViewModel(CampingMapViewModel viewModel)
        {
            this.MapViewModel = viewModel;
            DataContext = viewModel;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CampingMapViewModel.EditMode))
            {
                if (MapViewModel.EditMode)
                {
                    AddButton.Content = "Annuleren";
                }
                else
                {
                    AddButton.Content = "Toevoegen plaats";
                }
            }
        }

        private void Plaats_Click(object sender, RoutedEventArgs e)
        {
            MapViewModel.EditMode = !MapViewModel.EditMode;
        }

        private void Opslaan_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("opslaan knop klik");
            MapViewModel.Save();
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
