using CampingApplication.Business;
using CampingApplication.Business.BookingService;
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
        private readonly BookingService bookingService;

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

        private BookingRequest bookingRequest;

        public string FirstName
        {
            get => bookingRequest.FirstName;
            set
            {
                bookingRequest.FirstName = value;
                OnPropertyChanged(nameof(FirstName));
                if (FirstNameError != null) Revalidate(nameof(FirstName));
            }
        }
        public string? FirstNameError { get; private set; }

        public string LastName
        {
            get => bookingRequest.LastName;
            set
            {
                bookingRequest.LastName = value;
                OnPropertyChanged(nameof(LastName));
                if (LastNameError != null) Revalidate(nameof(LastName));
            }
        }
        public string? LastNameError { get; private set; }

        public string Email
        {
            get => bookingRequest.Email;
            set
            {
                bookingRequest.Email = value;
                OnPropertyChanged(nameof(Email));
                if (EmailError != null) Revalidate(nameof(Email));
            }
        }
        public string? EmailError { get; private set; }

        public string PhoneNumber
        {
            get => bookingRequest.PhoneNumber;
            set
            {
                bookingRequest.PhoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
                if (PhoneNumberError != null) Revalidate(nameof(PhoneNumber));
            }
        }
        public string? PhoneNumberError { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public BookingViewModel()
        {
            bookingRequest = new BookingRequest
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                FirstName = "",
                LastName = "",
                Email = "",
                PhoneNumber = "",
            };

            try
            {
                bookingService = ServiceProvider.Current.Resolve<BookingService>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                ServiceProvider.Current.RegisterInstance(new BookingService(new BookingRepostoryMock()));
                bookingService = ServiceProvider.Current.Resolve<BookingService>();
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task SubmitBooking()
        {
            ButtonState = ButtonState.Loading;
            try
            {
                bool success = await bookingService.BookAsync(bookingRequest);
                if (!success)
                    throw new Exception();
            }
            catch (BookingValidationException ex)
            {
                foreach (KeyValuePair<string, string> error in ex.Errors)
                {
                    SetError(error.Key, error.Value);
                }
            }

            ButtonState = ButtonState.Active;
        }

        private void Revalidate(string propertyName)
        {
            string? error = propertyName switch
            {
                nameof(FirstName) => BookingValidator.ValidateFirstName(FirstName),
                nameof(LastName) => BookingValidator.ValidateLastName(LastName),
                nameof(Email) => BookingValidator.ValidateEmail(Email),
                nameof(PhoneNumber) => BookingValidator.ValidatePhoneNumber(PhoneNumber),
                _ => null
            };

            SetError(propertyName, error);
        }

        private void SetError(string propertyName, string? error)
        {
            if (propertyName == nameof(FirstName)) FirstNameError = error;
            else if (propertyName == nameof(LastName)) LastNameError = error;
            else if (propertyName == nameof(Email)) EmailError = error;
            else if (propertyName == nameof(PhoneNumber)) PhoneNumberError = error;

            OnPropertyChanged($"{propertyName}Error");
        }
    }
}
