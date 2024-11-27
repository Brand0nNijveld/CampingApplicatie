using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampingApplication.Business
{
    public enum DateValidationResult
    {
        EndBeforeBegin,
        EndIsBegin,
        ValidDates

    }
    public class DateValidationService
    {
        public static DateValidationResult ValidateDates(DateTime beginDate, DateTime endDate)
        {
            if (endDate < beginDate)
            {
                return DateValidationResult.EndBeforeBegin;
            }
            else if (endDate == beginDate)
            {
                return DateValidationResult.EndIsBegin;
            }
            return DateValidationResult.ValidDates;
        }
    }
}
