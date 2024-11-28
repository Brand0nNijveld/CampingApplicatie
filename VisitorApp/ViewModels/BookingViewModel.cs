using CampingApplication.VisitorApp.Views.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CampingApplication.VisitorApp.ViewModels
{
    public class BookingViewModel : INotifyPropertyChanged
    {
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task SubmitBooking()
        {
            ButtonState = ButtonState.Loading;
            await Task.Delay(1000);
            ButtonState = ButtonState.Enabled;
            Debug.WriteLine("Button re-enabled");
        }
    }
}
