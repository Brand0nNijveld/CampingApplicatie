using CampingApplication.VisitorApp.ViewModels;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CampingApplication.VisitorApp.Views.Booking
{
    /// <summary>
    /// Interaction logic for BookingStepsPanel.xaml
    /// </summary>
    public partial class ActionPanel : UserControl
    {
        public ActionPanelViewModel? ViewModel { get; private set; }

        public ActionPanel()
        {
            InitializeComponent();
            TogglePanel(false, false);
            this.Visibility = Visibility.Collapsed;
        }

        public void SetViewModel(ActionPanelViewModel viewModel)
        {
            ViewModel = viewModel;
            ViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ViewModel.CurrentView))
                {
                    if (ViewModel.Views.Count == 0 || ViewModel.CurrentView < 0)
                    {
                        TogglePanel(false);
                        return;
                    }

                    ContentControl.Content = ViewModel.Views[ViewModel.CurrentView];
                    TogglePanel(true);
                }
            };
        }

        private void TogglePanel(bool show, bool transition = true)
        {
            if (show)
            {
                Visibility = Visibility.Visible;
            }

            var opacityAnimation = new DoubleAnimation
            {
                To = show ? 1 : 0,
                Duration = new Duration(TimeSpan.FromSeconds(transition ? 0.1 : 0)),
            };

            var translateYAnimation = new DoubleAnimation
            {
                From = show ? 50 : 0,
                To = show ? 0 : 50,
                Duration = new Duration(TimeSpan.FromSeconds(transition ? 0.1 : 0))
            };

            Storyboard.SetTarget(opacityAnimation, ActionPanelBackground);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTarget(translateYAnimation, ContentWindow);
            Storyboard.SetTargetProperty(translateYAnimation, new PropertyPath("RenderTransform.(TranslateTransform.Y)"));
            ContentWindow.RenderTransform = new TranslateTransform();

            var storyBoard = new Storyboard();
            storyBoard.Children.Add(opacityAnimation);
            storyBoard.Children.Add(translateYAnimation);

            storyBoard.Completed += (e, s) =>
            {
                Debug.WriteLine("Finished hiding action panel");
                if (!show)
                {
                    Visibility = Visibility.Collapsed;
                    ContentControl.Content = null;
                }
            };

            storyBoard.Begin();
        }
    }
}
