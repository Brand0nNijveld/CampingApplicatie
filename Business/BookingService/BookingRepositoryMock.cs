using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business.BookingService
{
    public class BookingRepositoryMock : IBookingRepository
    {
        public Task<IEnumerable<Booking>> GetBookingsInTimeFrameAsync(int campingSpotID, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public async Task SaveBookingAsync(BookingRequest request)
        {
            await Task.Delay(1000);
        }
    }
}
