﻿using CampingApplication.VisitorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CampingApplication.VisitorApp.Views.Map
{
    /// <summary>
    /// Interaction logic for CampingSpot.xaml
    /// </summary>
    public partial class CampingSpotView : UserControl
    {
        private readonly CampingSpotViewModel viewModel;

        public CampingSpotView(CampingSpotViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            DataContext = viewModel;

            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void CampingSpot_Clicked(object sender, MouseButtonEventArgs e)
        {
            viewModel.ShowDetails();
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.Available))
            {
                if (viewModel.Available)
                {
                    SpotBorder.Background = new SolidColorBrush(Colors.Yellow);
                }
                else
                {
                    SpotBorder.Background = new SolidColorBrush(Colors.Transparent);
                }
            }
        }
    }
}
