using CampingApplication.Business;
using CampingApplication.Business.CampingSpotService;
using Moq;

namespace UnitTests
{
    public class CampingSpotServiceTests
    {
        private CampingSpotService? service;

        [SetUp]
        public void Setup()
        {
            var mockRepository = new Mock<ICampingSpotRepository>();
            mockRepository
                .Setup(repo => repo.GetCampingSpots())
                .Returns(GetMockCampingSpots);

            service = new CampingSpotService(mockRepository.Object);
        }

        [TestCase("2024-11-18", "2024-11-23", 2)]
        [TestCase("2024-11-10", "2024-11-20", 3)]
        [TestCase("2024-11-25", "2024-12-5", 1)]
        [TestCase("2024-11-9", "2024-12-5", 0)]
        public void GetAvailableCampingSpots_ReturnsCorrectAvailableSpots(string startDateString, string endDateString, int result)
        {
            DateTime startDate = DateTime.Parse(startDateString);
            DateTime endDate = DateTime.Parse(endDateString);

            var availableSpots = CampingSpotService.GetAvailableSpots([.. service?.GetCampingSpots()], startDate, endDate);

            Assert.That(availableSpots.Count(), Is.EqualTo(result), $"There should be exactly {result} available spot(s)");
        }

        private IEnumerable<CampingSpot> GetMockCampingSpots()
        {
            return new List<CampingSpot>
            (
                [
            new CampingSpot(
                1,
                0,
                0,
                [
                    new(1, DateTime.Parse("2024-11-21"), DateTime.Parse("2024-11-26"))
                ]
            ),
            new CampingSpot(
                2,
                0,
                0,
                [
                    new(1, DateTime.Parse("2024-11-1"), DateTime.Parse("2024-11-9"))
                ]
            ),
            new CampingSpot(
                3,
                0,
                0,
                [
                    new(1, DateTime.Parse("2024-11-24"), DateTime.Parse("2024-11-28")),
                    new(1, DateTime.Parse("2024-12-1"), DateTime.Parse("2024-12-7"))
                ]
            )]);
        }
    }
}