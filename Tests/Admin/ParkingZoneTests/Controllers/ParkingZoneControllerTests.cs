using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingZoneApp.Areas.Admin;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services;
using ParkingZoneApp.ViewModels.ParkingZonesVMs;
using System.Text.Json;

namespace Tests.Admin.ParkingZoneTests.Controllers
{
    public class ParkingZoneControllerTests
    {
        private readonly Mock<IParkingZoneService> _zoneService;
        private readonly Mock<IReservationService> _reservationService;
        private readonly ParkingZoneController _controller;
        private readonly ParkingZone _parkingZoneTest;
        private readonly Reservation _reservationTest;
        private readonly ParkingSlot _parkingSlotTest;
        private readonly int Id = 1;

        public ParkingZoneControllerTests()
        {
            _zoneService = new Mock<IParkingZoneService>();
            _reservationService = new Mock<IReservationService>();
            _controller = new ParkingZoneController(_zoneService.Object, _reservationService.Object);
            _reservationTest = new Reservation();
            _parkingSlotTest = new() { Reservations = new[] { _reservationTest } };
            _parkingZoneTest = new()
            {
                Id = Id,
                Name = "Test",
                Address = "Test Address",
                ParkingSlots = new[] { _parkingSlotTest }
            };
        }

        #region Index
        [Fact]
        public void GivenNothing_WhenIndexIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            var expectedVMs = new List<ListItemVM>() { new(_parkingZoneTest) };
            var expectedParkingZones = new List<ParkingZone>() { _parkingZoneTest };

            _zoneService.Setup(x => x.GetAll()).Returns(expectedParkingZones);

            //Act
            var result = _controller.Index();

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            _zoneService.Verify(x => x.GetAll(), Times.Once);
            Assert.Equal(JsonSerializer.Serialize(expectedVMs), JsonSerializer.Serialize(model));
            Assert.NotNull(result);
        }
        #endregion

        #region Details
        [Fact]
        public void GivenParkingZoneId_WhenDetailsIsCalled_ThenReturnsVM()
        {
            //Arrange
            var ExpectedVM = new DetailsVM()
            {
                Id = Id,
                Name = "Test",
                Address = "Test Address",
            };
            _zoneService.Setup(x => x.GetById(Id)).Returns(_parkingZoneTest);

            //Act
            var viewresult = _controller.Details(Id);

            //Assert
            var model = Assert.IsType<ViewResult>(viewresult).Model;
            Assert.NotNull(viewresult);
            Assert.IsType<DetailsVM>(model);
            _zoneService.Verify(x => x.GetById(Id), Times.Once);
            Assert.Equal(JsonSerializer.Serialize(ExpectedVM), JsonSerializer.Serialize(model));
        }

        [Fact]
        public void GivenParkingZoneId_WhenDetailsIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _zoneService.Setup(x => x.GetById(Id));

            //Act
            var result = _controller.Details(Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _zoneService.Verify(x => x.GetById(Id), Times.Once);
        }
        #endregion

        #region Create
        [Fact]
        public void GivenNothing_WhenCreateIsCalled_ThenReturnsViewResult()
        {
            //Arrange

            //Act
            var viewresult = _controller.Create();

            //Assert
            Assert.IsType<ViewResult>(viewresult);
        }

        [Fact]
        public void GivenCreateVM_WhenCreateIsCalledToPost_ThenModelStateIsFalseAndReturnsVM()
        {
            //Arrange
            CreateVM createVM = new();
            _controller.ModelState.AddModelError("field", "property is required");

            //Act
            var result = _controller.Create(createVM);

            //Assert\
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<CreateVM>(model);
            Assert.NotNull(createVM);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenCreateVM_WhenCreateIsCalled_ThenModelStateIsTrueAndReturnsRedirectToAction()
        {
            //Arrange
            CreateVM createVM = new();
            _zoneService.Setup(x => x.Insert(It.IsAny<ParkingZone>()));

            //Act
            var redirectResult = _controller.Create(createVM);

            //Assert
            var result = Assert.IsType<RedirectToActionResult>(redirectResult);
            Assert.True(_controller.ModelState.IsValid);
            _zoneService.Verify(x => x.Insert(It.IsAny<ParkingZone>()), Times.Once);
        }
        #endregion

        #region Edit
        [Fact]
        public void GivenParkingZoneId_WhenEditIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _zoneService.Setup(x => x.GetById(Id));

            //Act
            var result = _controller.Edit(Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            _zoneService.Verify(x => x.GetById(Id));
        }

        [Fact]
        public void GivenParkingZoneId_WhenEditIsCalled_ThenReturnsVM()
        {
            //Arrange
            var ExpectedVM = new EditVM()
            {
                Id = Id,
                Name = "Test",
                Address = "Test Address"
            };
            _zoneService.Setup(x => x.GetById(Id)).Returns(_parkingZoneTest);

            //Act
            var result = _controller.Edit(Id);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<EditVM>(model);
            Assert.Equal(JsonSerializer.Serialize(ExpectedVM), JsonSerializer.Serialize(model));
            _zoneService.Verify(x => x.GetById(Id), Times.Once);
        }

        [Fact]
        public void GivenIdAndEditVM_WhenEditIsCalledToPost_ThenReturnsNotFound()
        {
            //Arrange
            EditVM editVM = new();

            //Act
            var result = _controller.Edit(Id, editVM);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GivenIdAndEditVM_WhenEditIsCalledToPost_ThenModelStateIsTrueAndReturnsRedirectToAction()
        {
            //Arrange
            _zoneService.Setup(x => x.GetById(Id)).Returns(_parkingZoneTest);

            EditVM editVM = new()
            {
                Id = Id,
                Name = "Test",
                Address = "Test Address"
            };

            //Act
            var result = _controller.Edit(Id, editVM);

            //Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.True(_controller.ModelState.IsValid);
            _zoneService.Verify(x => x.GetById(Id), Times.Once);
        }

        [Fact]
        public void GivenIdAndEditVM_WhenEditIsCalledToPost_ThenModelStateIsFalseAndReturnsView()
        {
            //Arrange
            EditVM editVM = new() { Id = Id };
            _controller.ModelState.AddModelError("field", "property is required");

            //Act
            var result = _controller.Edit(Id, editVM);

            //Assert
            Assert.False(_controller.ModelState.IsValid);
            Assert.IsType<ViewResult>(result);
        }
        #endregion

        #region Delete
        [Fact]
        public void GivenId_WhenDeleteIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _zoneService.Setup(x => x.GetById(Id));

            //Act
            var result = _controller.Delete(Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            _zoneService.Verify(x => x.GetById(Id), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteIsCalled_ThenReturnsView()
        {
            //Arrange
            _zoneService.Setup(x => x.GetById(Id)).Returns(_parkingZoneTest);

            //Act
            var result = _controller.Delete(Id);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.Equal(JsonSerializer.Serialize(_parkingZoneTest), JsonSerializer.Serialize(model));
            _zoneService.Verify(x => x.GetById(Id), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteConfirmedIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _zoneService.Setup(x => x.GetById(Id));

            //Act
            var result = _controller.DeleteConfirmed(Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            _zoneService.Verify(x => x.GetById(Id), Times.Once);
        }

        [Fact]
        public void GivenId_WhenDeleteConfirmedIsCalled_ThenReturnsRedirectToAction()
        {
            //Arrange
            _zoneService.Setup(x => x.GetById(Id)).Returns(_parkingZoneTest);
            _zoneService.Setup(x => x.Delete(_parkingZoneTest));

            //Act
            var result = _controller.DeleteConfirmed(Id);

            //Assert
            Assert.IsType<RedirectToActionResult>(result);
            _zoneService.Verify(x => x.GetById(Id), Times.Once);
            _zoneService.Verify(x => x.Delete(_parkingZoneTest), Times.Once);
        }
        #endregion

        #region Current Cars
        [Fact]
        public void GivenId_WhenCurrentCarsIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            _zoneService.Setup(x => x.GetById(Id));

            //Act
            var result = _controller.CurrentCars(Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _zoneService.Verify(x => x.GetById(Id), Times.Once);
        }

        [Fact]
        public void GivenId_WhenCurrentCarsIsCalled_ThenReturnsViewResult()
        {
            List<Reservation> reservations = new();
            _zoneService.Setup(x => x.GetById(Id)).Returns(_parkingZoneTest);
            _reservationService.Setup(x => x.GetAllReservationsByParkingZoneId(_parkingZoneTest.Id)).Returns(reservations);

            //Act
            var result = _controller.CurrentCars(Id);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsAssignableFrom<IEnumerable<ParkingZoneApp.ViewModels.ParkingSlotVMs.CurrentCarsVM>>(model);
            _zoneService.Verify(x => x.GetById(Id), Times.Once);
            _reservationService.Verify(x => x.GetAllReservationsByParkingZoneId(_parkingZoneTest.Id), Times.Once);
        }
        #endregion
    }
}