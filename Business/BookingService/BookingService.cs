using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.BookingService
{
    public class BookingService
    {
        private IBookingRepository repository;

        public BookingService(IBookingRepository repository)
        {
            this.repository = repository;
        }

        public async Task BookAsync(BookingRequest request)
        {
            var errors = BookingValidator.ValidateRequest(request);

            if (errors.Count != 0)
            {
                throw new BookingValidationException(errors);
            }

            await repository.SaveBookingAsync(request);
        }

        public static int CalculateAmountOfNights(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).Days - 1;
        }

        public static float CalculateTotalPrice(int amountOfNights, float pricePerNight)
        {
            return amountOfNights * pricePerNight;
        }
    }
}
