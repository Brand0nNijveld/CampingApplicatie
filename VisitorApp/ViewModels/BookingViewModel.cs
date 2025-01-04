using CampingApplication.Business;
using CampingApplication.Business.BookingService;
using CampingApplication.VisitorApp.Models;
using CampingApplication.VisitorApp.Views.Components;
using DataAccess.Bookings;
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
    public delegate void BookingSuccessHandler();

    public class BookingViewModel : BaseViewModel
    {
        private readonly BookingService bookingService;

        public event BookingSuccessHandler? BookingSuccessful;

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

        public BookingViewModel(int ID, CampingMapModel mapModel)
        {
            bookingRequest = new BookingRequest
            {
                CampingSpotID = ID,
                StartDate = mapModel.StartDate,
                EndDate = mapModel.EndDate,
                FirstName = "",
                LastName = "",
                Email = "",
                PhoneNumber = "",
            };

            mapModel.PropertyChanged += MapModel_PropertyChanged;

            try
            {
                bookingService = ServiceProvider.Current.Resolve<BookingService>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                ServiceProvider.Current.RegisterInstance(new BookingService(new BookingRepositoryMock()));
                bookingService = ServiceProvider.Current.Resolve<BookingService>();
            }
        }

        private void MapModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is CampingMapModel model)
            {
                if (e.PropertyName == nameof(CampingMapModel.StartDate) || e.PropertyName == nameof(CampingMapModel.EndDate))
                {
                    bookingRequest.StartDate = model.StartDate;
                    bookingRequest.EndDate = model.EndDate;
                }
            }
        }

        public async Task SubmitBooking()
        {
            ButtonState = ButtonState.Loading;
            try
            {
                await bookingService.BookAsync(bookingRequest);

                BookingSuccessful?.Invoke();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Debug.WriteLine("Index out of range: " + ex.Message);
            }
            catch (BookingValidationException ex)
            {
                foreach (KeyValuePair<string, string> error in ex.Errors)
                {
                    SetError(error.Key, error.Value);
                }
            }
            catch (BookingException ex)
            {
                Debug.WriteLine(ex.Message);
                SystemError = "Boeken voor deze periode is niet meer mogelijk.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                SystemError = "Er is iets misgegaan bij het opslaan van de boeking. Probeer het later opnieuw.";
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
