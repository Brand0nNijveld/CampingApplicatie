using CampingApplication.EmployeeApp.ViewModels;
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

namespace CampingApplication.EmployeeApp.Views
{
    /// <summary>
    /// Interaction logic for CampingSpotView.xaml
    /// </summary>
    public partial class CampingSpotView : UserControl
    {
        private CampingSpotViewModel viewModel;

        public CampingSpotView(CampingSpotViewModel model)
        {
            InitializeComponent();
            viewModel = model;
            DataContext = model;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.Edited))
            {
                if ( viewModel.Edited)
                {
                    Box.Background = new SolidColorBrush(Colors.Azure);
                }
            }

        }
    }
}
