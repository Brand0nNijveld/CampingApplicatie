using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CampingApplication.Business;
using CampingApplication.Business.BookingService;
using CampingApplication.Business.LoginService;

namespace CampingApplication.EmployeeViewReservations.ViewModels
{
    public class EmployeeBookingViewModel : INotifyPropertyChanged
    {
        private BookingService bookingService;
        private List<Booking> _bookings;

        public bool HasBookings;

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<Booking> bookings
        {
            get => _bookings;
            set
            {
                if (_bookings != value)
                {
                    _bookings = value;
                    OnPropertyChanged(nameof(bookings));
                }
            }
        }

        


        public EmployeeBookingViewModel()
        {
            try
            {
                bookingService = ServiceProvider.Current.Resolve<BookingService>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                ServiceProvider.Current.RegisterInstance(new BookingRepositoryMock());
                bookingService = ServiceProvider.Current.Resolve<BookingService>();
            }
        }

        

        public List<Booking> GetBookings()
        {
            this.bookings = bookingService.GetBookings();
            return bookings;
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
