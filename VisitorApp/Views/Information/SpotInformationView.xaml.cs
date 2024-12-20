using CampingApplication.VisitorApp.ViewModels;
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
    /// Interaction logic for SpotInformationView.xaml
    /// </summary>
    public partial class SpotInformationView : UserControl
    {
        public SpotInformationViewModel? viewModel;

        public SpotInformationView()
        {
            InitializeComponent();
            VisualStateManager.GoToState(this, "Closed", false);
        }

        public void SetViewModel(SpotInformationViewModel viewModel)
        {
            this.viewModel = viewModel;
            DataContext = this.viewModel;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (viewModel == null)
                return;

            if (e.PropertyName == nameof(viewModel.Open))
            {
                if (viewModel.Open)
                {
                    VisualStateManager.GoToState(this, "Open", true);
                }
                else
                {
                    VisualStateManager.GoToState(this, "Closed", true);
                }
            }
        }

        private void CloseButton_Clicked(object sender, MouseButtonEventArgs e)
        {
            viewModel?.Close();
        }
    }
}
