using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.BookingService
{
    public interface IBookingRepository
    {
        public Task<bool> SaveBookingAsync(BookingRequest request);
    }
}
