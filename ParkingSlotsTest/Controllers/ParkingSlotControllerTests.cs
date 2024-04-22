using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingZoneApp.Areas.Admin;
using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services;
using ParkingZoneApp.ViewModels.ParkingSlotsVMs;
using System.Text.Json;

namespace ParkingSlotsTest.Controllers
{
    public class ParkingSlotControllerTests
    {
        private readonly Mock<IParkingZoneService> _zoneService;
        private readonly Mock<IParkingSlotService> _slotService;
        private readonly ParkingSlotController _controller;
        private readonly ParkingSlot _parkingSlotsTest;
        private readonly int Id = 1;

        public ParkingSlotControllerTests()
        {
            _zoneService = new Mock<IParkingZoneService>();
            _slotService = new Mock<IParkingSlotService>();
            _controller = new ParkingSlotController(_slotService.Object, _zoneService.Object);
            _parkingSlotsTest = new()
            {
                Id = Id,
                Number = 1,
                Category = SlotCategoryEnum.Econom,
                IsAvilableForBooking = true,
                ParkingZoneId = 1
            };
        }

        [Fact]
        public void GivenNothing_WhenIndexIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            var expectedVMs = new List<ListItemVM>() { new(_parkingSlotsTest) };
            var expectedParkingSlots = new List<ParkingSlot>() { _parkingSlotsTest };

            _slotService.Setup(x => x.GetByParkingZoneId(Id)).Returns(expectedParkingSlots);
            _zoneService.Setup(x => x.GetById(Id)).Returns(new ParkingZone());

            //Act
            var result = _controller.Index(Id);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;

            _slotService.Verify(x => x.GetByParkingZoneId(Id), Times.Once);
            _zoneService.Verify(x => x.GetById(Id), Times.Once);
            Assert.Equal(JsonSerializer.Serialize(expectedVMs), JsonSerializer.Serialize(model));
            Assert.NotNull(result);
        }
    }
}
