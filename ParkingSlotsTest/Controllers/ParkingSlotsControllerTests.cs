using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingZoneApp.Areas.Admin;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services;
using ParkingZoneApp.ViewModels.ParkingSlotsVMs;
using System.Text.Json;

namespace ParkingSlotsTest.Controllers
{
    public class ParkingSlotsControllerTests
    {
        private readonly Mock<IParkingSlotsService> _service;
        private readonly ParkingSlotsController _controller;
        private readonly ParkingSlots _parkingSlotsTest;
        private readonly int Id = 1;

        public ParkingSlotsControllerTests()
        {
            _service = new Mock<IParkingSlotsService>();
            _controller = new ParkingSlotsController(_service.Object);
            _parkingSlotsTest = new()
            {
                Id = Id,
                Number = 1,
                Category = "Standart",
                IsAvilableForBooking = true,
                FeePerHour = "1000",
                ParkingZoneId = 1
            };
        }

        [Fact]
        public void GivenNothing_WhenIndexIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            var expectedVMs = new List<ListItemVM>() { new(_parkingSlotsTest) };
            var expectedParkingSlots = new List<ParkingSlots>() { _parkingSlotsTest };

            _service.Setup(x => x.GetAll()).Returns(expectedParkingSlots);

            //Act
            var result = _controller.Index(Id);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            _service.Verify(x => x.GetAll(), Times.Once);
            Assert.Equal(JsonSerializer.Serialize(expectedVMs), JsonSerializer.Serialize(model));
            Assert.NotNull(result);
        }
    }
}
