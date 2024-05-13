using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingZoneApp.ViewModels.ReservationVMs;
using ParkingZoneApp.Areas.User.Controllers;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services;
using System.Security.Claims;
using System.Text.Json;

namespace Tests.User.ReservationTests.Controllers
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
            _controller = new ReservationController(_zoneService.Object, _slotService.Object, _reservationService.Object);
            _reservationsTest = new()
            {
                ParkingSlotId = Id
            };
        }

        #region Index

        [Fact]
        public void GivenNothing_WhenIndexIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            var reservationsHistoryVMs = new List<ReservationHistoryListItemVM>();
            var reservations = new List<Reservation>();
            string userId = "asgfsev-ert634g35-erfbge-4gbedfbdr-g54ebsv";

            var testUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = testUser }
            };

            _reservationService.Setup(x => x.ReservationsByUserId(userId)).Returns(reservations);

            //Act
            var result = _controller.Index();

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsAssignableFrom<IOrderedEnumerable<ReservationHistoryListItemVM>>(model);
            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(reservationsHistoryVMs), JsonSerializer.Serialize(model));
            _reservationService.Verify(x => x.ReservationsByUserId(userId), Times.Once());
        }
        #endregion

        #region Prolong

        [Fact]
        public void GivenReservationId_WhenProlongIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _reservationService.Setup(x => x.GetById(Id));

            //Act
            var result = _controller.Prolong(Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _reservationService.Verify(x => x.GetById(Id), Times.Once);
        }

        [Fact]
        public void GivenReservationId_WhenProlongIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            ParkingZone zone = new();
            ParkingSlot slot = new() { ParkingZone = zone };
            Reservation reservation = new() { ParkingSlot = slot };
            _reservationService.Setup(x => x.GetById(Id)).Returns(reservation);

            //Act
            var result = _controller.Prolong(Id);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<ProlongVM>(model);
            _reservationService.Verify(x => x.GetById(Id), Times.Once);
        }

        [Fact]
        public void GivenProlongVM_WhenPostProlongIsCalled_ThenSlotIsNotFreeAndModelStateIsFalseAndReturnsViewResult()
        {
            //Arrange
            ParkingSlot slot = new();
            Reservation reservation = new()
            {
                ParkingSlot = slot
            };
            ProlongVM vm = new()
            {
                ReservationId = 1,
                EndTime = DateTime.Now,
                AddHours = 2,
            };
            _reservationService.Setup(x => x.GetById(vm.ReservationId)).Returns(reservation);
            _slotService.Setup(x => x.IsSlotFreeForReservation(reservation.ParkingSlot, vm.EndTime, vm.AddHours)).Returns(false);
            _controller.ModelState.AddModelError("AddHours", "Slot is not free for chosen time period");

            //Act
            var result = _controller.Prolong(vm);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<ProlongVM>(model);
            Assert.False(_controller.ModelState.IsValid);
            Assert.Equal(JsonSerializer.Serialize(vm), JsonSerializer.Serialize(model));
            _reservationService.Verify(x => x.GetById(vm.ReservationId), Times.Once);
            _slotService.Verify(x => x.IsSlotFreeForReservation(reservation.ParkingSlot, vm.EndTime, vm.AddHours), Times.Once);
        }

        [Fact]
        public void GivenProlongVM_WhenPostProlongIsCalled_ThenReturnsRedirectToActionResult()
        {
            //Arrange
            ParkingSlot slot = new();
            Reservation reservation = new()
            {
                ParkingSlot = slot
            };
            ProlongVM vm = new()
            {
                ReservationId = 1,
                EndTime = DateTime.Now,
                AddHours = 2
            };
            _reservationService.Setup(x => x.GetById(vm.ReservationId)).Returns(reservation);
            _slotService.Setup(x => x.IsSlotFreeForReservation(reservation.ParkingSlot, vm.EndTime, vm.AddHours)).Returns(true);
            _reservationService.Setup(x => x.ProlongReservation(reservation, vm.AddHours));

            //Act
            var result = _controller.Prolong(vm);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
            _reservationService.Verify(x => x.GetById(vm.ReservationId), Times.Once);
            _slotService.Verify(x => x.IsSlotFreeForReservation(reservation.ParkingSlot, vm.EndTime, vm.AddHours), Times.Once);
            _reservationService.Verify(x => x.ProlongReservation(reservation, vm.AddHours), Times.Once);
        }
        #endregion
    }
}
