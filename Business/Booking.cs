using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business
{
    public class Booking
    {
        public int ID { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public Booking(int ID, DateTime startDate, DateTime endDate)
        {
            this.ID = ID;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }
    }
}
