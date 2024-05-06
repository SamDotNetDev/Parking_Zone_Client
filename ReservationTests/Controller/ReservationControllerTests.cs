using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingZoneApp.Controllers;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services;
using ParkingZoneApp.ViewModels.Reservation;

namespace ReservationTests.Controller
{
    public class ReservationControllerTests
    {
        private readonly Mock<IReservationService> _reservationService;
        private readonly Mock<IParkingSlotService> _slotService;
        private readonly Mock<IParkingZoneService> _zoneService;
        private readonly ReservationController _controller;
        private readonly Reservation _reservationsTest;
        private readonly int Id = 1;

        public ReservationControllerTests()
        {
            _reservationService = new Mock<IReservationService>();
            _zoneService = new Mock<IParkingZoneService>();
            _slotService = new Mock<IParkingSlotService>();
            _controller = new ReservationController(_reservationService.Object, _slotService.Object, _zoneService.Object);
            _reservationsTest = new() { };
        }

        #region FreeSlots
        [Fact]
        public void GivenNothing_WhenFreeSlotsIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            List<ParkingZone> zones = new();
            _zoneService.Setup(x => x.GetAll()).Returns(zones);

            //Act
            var result = _controller.FreeSlots();

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<FreeSlotsVM>(model);
            _zoneService.Verify(x  => x.GetAll(), Times.Once);
        }

        [Fact]
        public void GivenFreeSlotsVM_WhenFreeSlotsIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            ParkingZone zone = new() {Id = Id, Name = "Test" };
            FreeSlotsVM vm = new() 
            { 
                ParkingZoneId = Id, 
                StartTime = DateTime.Now, 
                Duration = 2 
            };
            List<ParkingSlot> slots = new();
            _zoneService.Setup(x => x.GetById(vm.ParkingZoneId)).Returns(zone);
            _slotService.Setup(x => x.GetFreeByParkingZoneIdAndPeriod(vm.ParkingZoneId, vm.StartTime, vm.Duration)).Returns(slots);

            //Act
            var result = _controller.FreeSlots(vm);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<FreeSlotsVM>(model);
            _zoneService.Verify(x => x.GetById(Id), Times.Once);
            _slotService.Verify(x => x.GetFreeByParkingZoneIdAndPeriod(vm.ParkingZoneId, vm.StartTime, vm.Duration), Times.Once);
        }
        #endregion
    }
}
