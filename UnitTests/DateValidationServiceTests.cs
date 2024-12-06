using CampingApplication.Business;

namespace UnitTests
{
    public class DateValidationServiceTests
    {
        [TestCase("2024-11-25","2024-11-20",DateValidationResult.EndBeforeBegin)]
        [TestCase("2024-11-20","2024-11-20",DateValidationResult.EndIsBegin)]
        [TestCase("2024-11-20", "2024-11-25", DateValidationResult.ValidDates)]
        public void ValidateDates_ReturnsCorrectResult(string startDateString, string endDateString, DateValidationResult result)
        {
            DateTime startDate = DateTime.Parse(startDateString);
            DateTime endDate = DateTime.Parse(endDateString);


            var dateResult = DateValidationService.ValidateDates(startDate, endDate);
            Assert.That(result, Is.EqualTo(dateResult));
        }
    }
}