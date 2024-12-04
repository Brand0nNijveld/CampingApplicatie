using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.BookingService
{
    public class BookingRepositoryMock : IBookingRepository
    {
        public async Task SaveBookingAsync(BookingRequest request)
        {
            await Task.Delay(1000);
        }
    }
}
