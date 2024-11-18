using CampingApplication.Business;

namespace UnitTests
{
    public class CampingAvailabilityServiceTests
    {
        private CampingAvailabilityService service;

        [SetUp]
        public void Setup()
        {
            CampingSpot[] spots =
            [
            new CampingSpot(
                1,
                [
                    new(DateTime.Parse("2024-11-21"), DateTime.Parse("2024-11-26"))
                ]
            ),
            new CampingSpot(
                2,
                [
                    new(DateTime.Parse("2024-11-1"), DateTime.Parse("2024-11-9"))
                ]
            ),
            new CampingSpot(
                3,
                [
                    new(DateTime.Parse("2024-11-24"), DateTime.Parse("2024-11-28")),
                    new(DateTime.Parse("2024-12-1"), DateTime.Parse("2024-12-7"))
                ]
            ),
            ];

            service = new CampingAvailabilityService(spots);
        }

        [TestCase("2024-11-18", "2024-11-23", 2)]
        [TestCase("2024-11-10", "2024-11-20", 3)]
        [TestCase("2024-11-25", "2024-12-5", 1)]
        [TestCase("2024-11-9", "2024-12-5", 0)]
        public void GetAvailableCampingSpots_ReturnsCorrectAvailableSpots(string startDateString, string endDateString, int result)
        {
            DateTime startDate = DateTime.Parse(startDateString);
            DateTime endDate = DateTime.Parse(endDateString);

            var availableSpots = service.GetAvailableCampingSpots(startDate, endDate);

            Assert.That(availableSpots.Count(), Is.EqualTo(result), $"There should be exaclty {result} available spot(s)");
        }
    }
}