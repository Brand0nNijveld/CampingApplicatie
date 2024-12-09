using CampingApplication.Business;
using CampingApplication.Business.LoginService;
using CampingApplication.EmployeeApp.Components;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CampingApplication.EmployeeApp.ViewModels
{
    public delegate void LoginSuccessHandler();

    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly LoginService loginService;

        public event LoginSuccessHandler? LoginSuccessful;

        private ButtonState buttonState;
        public ButtonState ButtonState
        {
            get => buttonState;
            set
            {
                if (buttonState != value)
                {
                    buttonState = value;
                    OnPropertyChanged(nameof(ButtonState));
                }
            }
        }

        private string username = string.Empty;
        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
                if (UsernameError != null) Revalidate(nameof(Username));
            }
        }
        public string? UsernameError { get; private set; }

        private string password = string.Empty;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
                if (PasswordError != null) Revalidate(nameof(Password));
            }
        }
        public string? PasswordError { get; private set; }

        private string? systemError;
        public string? SystemError
        {
            get => systemError;
            set
            {
                systemError = value;
                OnPropertyChanged(nameof(SystemError));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public LoginViewModel()
        {
            try
            {
                loginService = ServiceProvider.Current.Resolve<LoginService>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                ServiceProvider.Current.RegisterInstance(new LoginRepositoryMock());
                loginService = ServiceProvider.Current.Resolve<LoginService>();
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task SubmitLogin()
        {
            ButtonState = ButtonState.Loading;
            try
            {
                bool isAuthenticated = await loginService.AuthenticateAsync(Username, Password);

                if (isAuthenticated)
                {
                    LoginSuccessful?.Invoke();
                }
                else
                {
                    SystemError = "Invalid username or password.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                SystemError = "An error occurred during login. Please try again later.";
            }

            ButtonState = ButtonState.Active;
        }

        private void Revalidate(string propertyName)
        {
            string? error = propertyName switch
            {
                nameof(Username) => LoginValidator.ValidateUsername(Username),
                nameof(Password) => LoginValidator.ValidatePassword(Password),
                _ => null
            };

            SetError(propertyName, error);
        }

        private void SetError(string propertyName, string? error)
        {
            if (propertyName == nameof(Username)) UsernameError = error;
            else if (propertyName == nameof(Password)) PasswordError = error;

            OnPropertyChanged($"{propertyName}Error");
        }
    }
}
