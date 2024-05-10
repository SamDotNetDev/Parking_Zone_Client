using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingZoneApp.Controllers;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services;
using ParkingZoneApp.ViewModels.ReservationVMs;
using System.Security.Claims;
using System.Text.Json;

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
            _reservationsTest = new() 
            {
                ParkingSlotId = Id
            };
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
            ParkingZone zoneTest = new();
            List<ParkingZone> zones = new() { zoneTest };
            FreeSlotsVM vm = new() 
            { 
                ParkingZoneId = Id, 
                StartTime = DateTime.Now, 
                Duration = 2 
            };
            List<ParkingSlot> slots = new();
            _zoneService.Setup(x => x.GetAll()).Returns(zones);
            _slotService.Setup(x => x.GetFreeByParkingZoneIdAndPeriod(vm.ParkingZoneId, vm.StartTime, vm.Duration)).Returns(slots);

            //Act
            var result = _controller.FreeSlots(vm);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<FreeSlotsVM>(model);
            Assert.Equal(JsonSerializer.Serialize(vm), JsonSerializer.Serialize(model));
            _zoneService.Verify(x => x.GetAll(), Times.Once);
            _slotService.Verify(x => x.GetFreeByParkingZoneIdAndPeriod(vm.ParkingZoneId, vm.StartTime, vm.Duration), Times.Once);
        }
        #endregion

        #region Reserve
        [Fact]
        public void GivenSlotIdAndStartTimeAndDuration_WhenReserveIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            int duration = 1;
            DateTime dateTime = DateTime.Now;
            _slotService.Setup(x => x.GetById(Id));

            //Act
            var result = _controller.Reserve(Id, dateTime, duration);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _slotService.Verify(x => x.GetById(Id), Times.Once);
        }

        [Fact]
        public void GivenSlotIdAndStartTimeAndDuration_WhenReserveIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            ParkingZone zone = new ();
            ParkingSlot slot = new ();
            slot.ParkingZone = zone;
            int duration = 1;
            DateTime dateTime = DateTime.Now;
            _slotService.Setup(x => x.GetById(Id)).Returns(slot);

            //Act
            var result = _controller.Reserve(Id, dateTime, duration);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<ReserveVM>(model);
            _slotService.Verify(x => x.GetById(Id), Times.Once);
        }

        [Fact]
        public void GivenReserveViewModel_WhenPostReserveIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            ReserveVM vm = new ReserveVM() { ParkingSlotId = Id };
            _slotService.Setup(x => x.GetById(vm.ParkingSlotId));

            //Act
            var result = _controller.Reserve(vm);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _slotService.Verify(x => x.GetById(vm.ParkingSlotId), Times.Once);
        }

        [Fact]
        public void GivenReserveViewModel_WhenPostReserveIsCalled_ThenModelStateIsFalseAndReturnsViewResult()
        {
            //Arrange
            ParkingZone zone = new();
            ParkingSlot slot = new()
            {
                Id = Id,
                ParkingZone = zone,
                Number = 1
            };
            ReserveVM vm = new()
            {
                StartTime = DateTime.Now,
                Duration = 1,
                ParkingSlotId = Id
            };
            _slotService.Setup(x => x.GetById(vm.ParkingSlotId)).Returns(slot);
            _slotService.Setup(x => x.IsSlotFreeForReservation(slot, vm.StartTime, vm.Duration)).Returns(false);

            //Act
            var result = _controller.Reserve(vm);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<ReserveVM>(model);
            Assert.False(_controller.ModelState.IsValid);
            Assert.Equal(JsonSerializer.Serialize(vm), JsonSerializer.Serialize(model));
            _slotService.Verify(x => x.GetById(vm.ParkingSlotId), Times.Once);
            _slotService.Verify(x => x.IsSlotFreeForReservation(slot, vm.StartTime, vm.Duration), Times.Once);
        }
        
        [Fact]
        public void GivenReserveViewModel_WhenPostReserveIsCalled_ThenModelStateIsTrueAndReturnsViewResult()
        {
            //Arrange
            ParkingZone zone = new();
            ParkingSlot slot = new()
            {
                Id = Id,
                ParkingZone = zone,
                Number = 1
            };
            ReserveVM vm = new()
            {
                StartTime = DateTime.Now,
                Duration = 1,
                ParkingSlotId = Id,
            };

            var mockClaimsPrincipal = CreateMockClaimsPrincipal();

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockClaimsPrincipal }
            };

            _controller.ControllerContext = controllerContext;

            _slotService.Setup(x => x.GetById(vm.ParkingSlotId)).Returns(slot);
            _slotService.Setup(x => x.IsSlotFreeForReservation(slot, vm.StartTime, vm.Duration)).Returns(true);
            _reservationService.Setup(x => x.Insert(It.IsAny<Reservation>()));

            //Act
            var result = _controller.Reserve(vm);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<ReserveVM>(model);
            Assert.True(_controller.ModelState.IsValid);
            Assert.Equal(JsonSerializer.Serialize(vm), JsonSerializer.Serialize(model));
            Assert.Equal(_controller.ViewBag.SuccessMessage, "Reservation successful.");
            _slotService.Verify(x => x.GetById(vm.ParkingSlotId), Times.Once);
            _slotService.Verify(x => x.IsSlotFreeForReservation(slot, vm.StartTime, vm.Duration), Times.Once);
            _reservationService.Verify(x => x.Insert(It.IsAny<Reservation>()), Times.Once);
        }

        private ClaimsPrincipal CreateMockClaimsPrincipal()
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, "UserId")
            };
            var identity = new ClaimsIdentity(claims);

            return new ClaimsPrincipal(identity);
        }
        #endregion
    }
}
