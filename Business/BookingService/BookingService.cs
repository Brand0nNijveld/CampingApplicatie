﻿using System;
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
            Dictionary<string, string> errors;
            try
            {
                errors = BookingValidator.ValidateRequest(request);
            }
            catch (Exception ex)
            {
                throw new Exception("Error during validation: " + ex.Message);
            }

            if (errors.Count != 0)
            {
                throw new BookingValidationException(errors);
            }

            await repository.SaveBookingAsync(request);
        }

        public static int CalculateAmountOfNights(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).Days;
        }

        public static float CalculateTotalPrice(int amountOfNights, float pricePerNight)
        {
            return amountOfNights * pricePerNight;
        }
    }
}
