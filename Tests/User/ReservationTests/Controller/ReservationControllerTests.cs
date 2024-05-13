using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingZoneApp.ViewModels.ReservationVMs;
using ParkingZoneApp.Areas.User.Controllers;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services;
using System.Security.Claims;
using System.Text.Json;

namespace Tests.User.ReservationTests.Controller
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
    }
}
