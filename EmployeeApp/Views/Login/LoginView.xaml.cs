using CampingApplication.Business.LoginService;
using CampingApplication.EmployeeApp.ViewModels;
using CampingApplication.EmployeeApp.Views.MainMenu;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CampingApplication.EmployeeApp.Views.Login
{
    public partial class LoginView : UserControl
    {
        public LoginViewModel ViewModel { get; private set; }

        public LoginView()
        {
            InitializeComponent();

            if (App.Current is App app && Business.ServiceProvider.Current != null)
            {
                LoginService loginService = Business.ServiceProvider.Current.Resolve<LoginService>();
                ViewModel = new LoginViewModel();
            }
            else
            {
                Debug.WriteLine("ServiceProvider isn't initialized");
            }

            DataContext = ViewModel;

            if (ViewModel != null)
            {
                ViewModel.PropertyChanged += ViewModel_PropertyChanged;
                ViewModel.LoginSuccessful += OnLoginSuccessful;
            }

            SystemErrorText.MaxHeight = 0;
            SystemErrorText.Opacity = 0;
            SystemErrorText.MouseLeftButtonUp += SystemErrorText_MouseLeftButtonUp;
        }

        private void PasswordBoxControl_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null && sender is PasswordBox passwordBox)
            {
                ViewModel.Password = passwordBox.Password;
            }
        }

        private void OnLoginSuccessful()
        {
            Dispatcher.Invoke(() =>
            {
                var mainMenuView = new MainMenuView();
                var parentWindow = Window.GetWindow(this);

                if (parentWindow != null)
                {
                    parentWindow.Content = mainMenuView;
                }
            });
        }

        private void SystemErrorText_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ViewModel.SystemError != null)
            {
                ViewModel.SystemError = null;
            }
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.SystemError))
            {
                Debug.WriteLine(e.PropertyName + ": " + ViewModel.SystemError);
                if (!string.IsNullOrEmpty(ViewModel.SystemError))
                {
                    AnimateError(1, 60);
                }
                else
                {
                    AnimateError(0, 0);
                }
            }
        }

        private void AnimateError(double opacity, double MaxHeight)
        {
            var opacityAnimation = new DoubleAnimation
            {
                To = opacity,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                BeginTime = TimeSpan.FromSeconds(0.1),
            };

            var heightAnimation = new DoubleAnimation
            {
                To = MaxHeight,
                Duration = new Duration(TimeSpan.FromSeconds(0.1))
            };

            Storyboard.SetTarget(opacityAnimation, SystemErrorText);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTarget(heightAnimation, SystemErrorText);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath("MaxHeight"));

            var storyBoard = new Storyboard();
            storyBoard.Children.Add(opacityAnimation);
            storyBoard.Children.Add(heightAnimation);
            storyBoard.Begin();
        }

        public async void Submit_Login(object sender, RoutedEventArgs e)
        {
            await ViewModel.SubmitLogin();
        }
    }
}
