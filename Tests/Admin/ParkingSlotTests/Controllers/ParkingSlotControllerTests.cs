using Moq;
using System.Text.Json;
using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services;
using Microsoft.AspNetCore.Mvc;
using ParkingZoneApp.Areas.Admin;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;

namespace Tests.Admin.ParkingSlotTests.Controllers
{
    public class ParkingSlotControllerTests
    {
        private readonly Mock<IParkingZoneService> _zoneService;
        private readonly Mock<IParkingSlotService> _slotService;
        private readonly ParkingSlotController _controller;
        private readonly Reservation _reservationTest;
        private readonly ParkingSlot _parkingSlotsTest;
        private readonly int Id = 1;

        public ParkingSlotControllerTests()
        {
            _zoneService = new Mock<IParkingZoneService>();
            _slotService = new Mock<IParkingSlotService>();
            _controller = new ParkingSlotController(_slotService.Object, _zoneService.Object);
            _reservationTest = new() { StartTime = DateTime.Now.AddHours(-2) };
            _parkingSlotsTest = new()
            {
                Id = Id,
                Number = 1,
                Category = SlotCategoryEnum.Standart,
                IsAvailableForBooking = true,
                ParkingZoneId = 1,
                Reservations = new[] { _reservationTest }
            };
        }

        #region Index
        [Fact]
        public void GivenParkingZoneId_WhenIndexIsCalled_ThenReturnsNotFound()
        {
            //Arrange
            var expectedParkingSlots = new List<ParkingSlot>() { _parkingSlotsTest };

            _zoneService.Setup(x => x.GetById(Id));
            //Act
            var result = _controller.Index(Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _zoneService.Verify(x => x.GetById(Id), Times.Once);
        }

        [Fact]
        public void GivenParkingZoneId_WhenIndexIsCalled_ThenReturnsViewResult()
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

        [Fact]
        public void GivenParkingZoneIdAndCategoryAndIsSlotFree_WhenPostIndexIsCalled_ThenFilterDataAndReturnsPartialViewResult()
        {
            //Arrange
            List<ParkingSlot> slots = new() { _parkingSlotsTest };
            SlotCategoryEnum category = SlotCategoryEnum.VIP;
            bool IsSlotFree = true;
            _slotService.Setup(x => x.GetByParkingZoneId(Id)).Returns(slots);

            //Act
            var result = _controller.Index(Id, category, IsSlotFree);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<PartialViewResult>(result).Model;
            var viewName = Assert.IsType<PartialViewResult>(result).ViewName;
            Assert.IsAssignableFrom<List<ListItemVM>>(model);
            Assert.Equal("_FilteredSlotsPartial", viewName);
            _slotService.Verify(x => x.GetByParkingZoneId(Id), Times.Once);
        }

        #endregion

        #region Create
        [Fact]
        public void GivenParkingZoneId_WhenCreateIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            var createVM = new CreateVM() { ParkingZoneId = Id };
            //Act
            var result = _controller.Create(Id);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<CreateVM>(model);
            Assert.Equal(JsonSerializer.Serialize(createVM), JsonSerializer.Serialize(model));
        }

        [Fact]
        public void GivenCreateVM_WhenCreateIsCalledToPost_ThenModelStateIsFalseAndReturnsViewResult()
        {
            //Arrange
            CreateVM createVM = new();
            _controller.ModelState.AddModelError("field", "property is required");

            //Act
            var result = _controller.Create(createVM);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<CreateVM>(model);
            Assert.NotNull(createVM);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void GivenCreateVM_WhenCreateIsCalledToPost_ThenNumberIsNegativeAndModelStateIsFalseAndReturnsViewResult()
        {
            //Arrange
            CreateVM createVM = new() { ParkingZoneId = 1, Number = -1 };
            _slotService.Setup(x => x.ParkingSlotExists(createVM.ParkingZoneId, createVM.Number)).Returns(false);

            //Act
            var result = _controller.Create(createVM);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<CreateVM>(model);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _slotService.Verify(x => x.ParkingSlotExists(createVM.ParkingZoneId, createVM.Number), Times.Once);
        }

        [Fact]
        public void GivenCreateVM_WhenCreateIsCalledToPost_ThenSlotExistsAndModelStateIsFalseAndReturnsRedirectToAction()
        {
            //Arrange
            CreateVM createVM = new() { ParkingZoneId = 1, Number = 1 };
            _slotService.Setup(x => x.ParkingSlotExists(createVM.ParkingZoneId, createVM.Number)).Returns(true);

            //Act
            var result = _controller.Create(createVM);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<CreateVM>(model);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _slotService.Verify(x => x.ParkingSlotExists(createVM.ParkingZoneId, createVM.Number), Times.Once);
        }

        [Fact]
        public void GivenCreateVM_WhenCreateIsCalledToPost_ThenModelStateIsTrueReturnsRedirectToAction()
        {
            //Arrange
            CreateVM createVM = new()
            {
                Number = 1,
                Category = SlotCategoryEnum.Standart,
                IsAvailableForBooking = true,
                ParkingZoneId = 1
            };
            _slotService.Setup(x => x.ParkingSlotExists(createVM.ParkingZoneId, createVM.Number)).Returns(false);
            _slotService.Setup(x => x.Insert(It.IsAny<ParkingSlot>()));

            //Act
            var result = _controller.Create(createVM);

            //Assert
            var action = Assert.IsType<RedirectToActionResult>(result).ActionName;
            Assert.Equal("Index", action);
            Assert.NotNull(result);
            Assert.True(_controller.ModelState.IsValid);
            _slotService.Verify(x => x.ParkingSlotExists(createVM.ParkingZoneId, createVM.Number), Times.Once);
            _slotService.Verify(x => x.Insert(It.IsAny<ParkingSlot>()), Times.Once);
        }
        #endregion

        #region Edit
        [Fact]
        public void GivenParkingSlotId_WhenEditIsCalled_ThenReturnsNotFoundResult()
        {
            //Arrange
            _slotService.Setup(x => x.GetById(Id));

            //Act
            var result = _controller.Edit(Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GivenParkingSlotId_WhenEditIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            EditVM editVM = new()
            {
                Id = Id,
                Number = 1,
                Category = SlotCategoryEnum.Standart,
                IsAvailableForBooking = true,
                ParkingZoneId = 1,
                IsInUse = false
            };
            _slotService.Setup(x => x.GetById(Id)).Returns(_parkingSlotsTest);

            //Act
            var result = _controller.Edit(Id);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<EditVM>(model);
            Assert.Equal(JsonSerializer.Serialize(editVM), JsonSerializer.Serialize(model));
        }

        [Fact]
        public void GivenParkingSlotIdAndEditVM_WhenEditIsCalledToPost_ThenReturnsNotFoundResult()
        {
            //Arrange
            EditVM editVM = new() { Id = 2, IsInUse = false };

            //Act
            var result = _controller.Edit(Id, editVM);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GivenParkingSlotIdAndEditVM_WhenEditIsCalledToPost_ThenModelStateIsFalseAndReturnsViewResult()
        {
            //Arrange
            EditVM editVM = new() { Id = Id, Number = 2, IsInUse = false };
            _slotService.Setup(x => x.GetById(Id)).Returns(_parkingSlotsTest);
            _slotService.Setup(x => x.ParkingSlotExists(editVM.ParkingZoneId, editVM.Number)).Returns(true);
            _controller.ModelState.AddModelError("field", "property is required");

            //Act
            var result = _controller.Edit(Id, editVM);

            //Assert
            Assert.False(_controller.ModelState.IsValid);
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<EditVM>(model);
            Assert.Equal(JsonSerializer.Serialize(editVM), JsonSerializer.Serialize(model));
            _slotService.Verify(x => x.GetById(Id), Times.Once());
            _slotService.Verify(x => x.ParkingSlotExists(editVM.ParkingZoneId, editVM.Number));
        }

        [Fact]
        public void GivenParkingSlotIdAndEditVM_WhenEditIsCalledToPost_ThenParkingSlotIsInUseIsFalseAndModelstateIsFalseAndReturnsViewResult()
        {
            //Arrange
            Reservation reservation = new() { StartTime = DateTime.Now.AddHours(1) };
            ParkingSlot parkingSlot = new() { Reservations = new[] { reservation } };
            EditVM editVM = new() { Id = Id, Number = 2, ParkingZoneId = 1 };
            _slotService.Setup(x => x.GetById(Id)).Returns(parkingSlot);
            _slotService.Setup(x => x.ParkingSlotExists(editVM.ParkingZoneId, editVM.Number)).Returns(false);
            _controller.ModelState.AddModelError("Category", "Slot is in use, category cannot be modified");
            //Act
            var result = _controller.Edit(Id, editVM);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<EditVM>(model);
            Assert.Equal(JsonSerializer.Serialize(editVM), JsonSerializer.Serialize(model));
            _slotService.Verify(x => x.GetById(Id), Times.Once());
            _slotService.Verify(x => x.ParkingSlotExists(editVM.ParkingZoneId, editVM.Number), Times.Once);
        }

        [Fact]
        public void GivenParkingSlotIdAndEditVM_WhenEditIsCalledToPost_ThenParkingSlotExistsAndNumerIsNotSameAndModelStateIsFalseAndReturnsViewResult()
        {
            //Arrange
            EditVM editVM = new() { Id = Id, Number = 2, ParkingZoneId = 1 };
            _slotService.Setup(x => x.GetById(Id)).Returns(_parkingSlotsTest);
            _slotService.Setup(x => x.ParkingSlotExists(editVM.ParkingZoneId, editVM.Number)).Returns(true);
            //Act
            var result = _controller.Edit(Id, editVM);

            //Assert
            Assert.False(_controller.ModelState.IsValid);
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<EditVM>(model);
            Assert.Equal(JsonSerializer.Serialize(editVM), JsonSerializer.Serialize(model));
            _slotService.Verify(x => x.GetById(Id), Times.Once());
            _slotService.Verify(x => x.ParkingSlotExists(editVM.ParkingZoneId, editVM.Number), Times.Once);
        }

        [Fact]
        public void GivenParkingSlotIdAndEditVM_WhenEditIsCalledToPost_ThenParkingSlotModelStateIsTrueAndReturnsRedirectToAction()
        {
            //Arrange
            EditVM editVM = new() { Id = Id, Number = 1, ParkingZoneId = 1, IsInUse = false };
            _slotService.Setup(x => x.GetById(Id)).Returns(_parkingSlotsTest);
            _slotService.Setup(x => x.ParkingSlotExists(editVM.ParkingZoneId, editVM.Number)).Returns(false);
            _slotService.Setup(x => x.Update(_parkingSlotsTest));
            //Act
            var result = _controller.Edit(Id, editVM);

            //Assert
            Assert.True(_controller.ModelState.IsValid);
            Assert.NotNull(result);
            var action = Assert.IsType<RedirectToActionResult>(result).ActionName;
            Assert.Equal("Index", action);
            _slotService.Verify(x => x.GetById(Id), Times.Once());
            _slotService.Verify(x => x.ParkingSlotExists(editVM.ParkingZoneId, editVM.Number), Times.Once);
            _slotService.Verify(x => x.Update(_parkingSlotsTest));
        }
        #endregion

        #region Details
        [Fact]
        public void GivenParkingSlotId_WhenDetailsIsCalled_ThenReturnsNotFoundResult()
        {
            //Arrange
            _slotService.Setup(x => x.GetById(Id));

            //Act
            var result = _controller.Details(Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _slotService.Verify(x => x.GetById(Id), Times.Once);
        }

        [Fact]
        public void GivenParkingSlotId_WhenDetailsIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            DetailsVM detailsVM = new()
            {
                Id = Id,
                Number = 1,
                IsAvailableForBooking = true,
                Category = SlotCategoryEnum.Standart
            };
            _slotService.Setup(x => x.GetById(Id)).Returns(_parkingSlotsTest);

            //Act
            var result = _controller.Details(Id);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<DetailsVM>(model);
            Assert.Equal(JsonSerializer.Serialize(detailsVM), JsonSerializer.Serialize(model));
            _slotService.Verify(x => x.GetById(Id), Times.Once);
        }
        #endregion

        #region Delete
        [Fact]
        public void GivenParkingSlotId_WhenDeleteIsCalled_ThenReturnsNotFoundResult()
        {
            //Arrange
            _slotService.Setup(x => x.GetById(Id));

            //Act
            var result = _controller.Delete(Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _slotService.Verify(x => x.GetById(Id), Times.Once());
        }

        [Fact]
        public void GivenParkingSlotId_WhenDeleteIsCalled_ThenReturnsViewResult()
        {
            //Arrange
            _slotService.Setup(x => x.GetById(Id)).Returns(_parkingSlotsTest);

            //Act
            var result = _controller.Delete(Id);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<ParkingSlot>(model);
            Assert.Equal(JsonSerializer.Serialize(_parkingSlotsTest), JsonSerializer.Serialize(model));
            _slotService.Verify(x => x.GetById(Id), Times.Once());
        }

        [Fact]
        public void GivenParkingSlotId_WhenDeleteConfirmedIsCalled_ThenReturnsNotFoundResult()
        {
            //Arrange
            _slotService.Setup(x => x.GetById(Id));

            //Act
            var result = _controller.DeleteConfirmed(Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _slotService.Verify(x => x.GetById(Id), Times.Once());
        }

        [Fact]
        public void GivenParkingSlotId_WhenDeleteConfirmedIsCalled_ThenIsInUseIsFalseAndReturnsNotFoundResult()
        {
            //Arrange
            Reservation reservation = new() { StartTime = DateTime.Now.AddHours(1) };
            ParkingSlot parkingSlot = new() { Reservations = new[] { reservation } };
            _slotService.Setup(x => x.GetById(Id)).Returns(parkingSlot);

            //Act
            var result = _controller.DeleteConfirmed(Id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _slotService.Verify(x => x.GetById(Id), Times.Once());
        }

        [Fact]
        public void GivenParkingSlotId_WhenDeleteConfirmedIsCalled_ThenReturnsRedirectToAction()
        {
            //Arrange
            Reservation reservation = new() { StartTime = DateTime.Now.AddHours(-1) };
            ParkingSlot parkingSlot = new() { Reservations = new[] { reservation } };
            _slotService.Setup(x => x.GetById(Id)).Returns(parkingSlot);
            _slotService.Setup(x => x.Delete(parkingSlot));

            //Act
            var result = _controller.DeleteConfirmed(Id);

            //Assert
            Assert.NotNull(result);
            var action = Assert.IsType<RedirectToActionResult>(result).ActionName;
            Assert.Equal("Index", action);
            _slotService.Verify(x => x.GetById(Id), Times.Once());
            _slotService.Verify(x => x.Delete(parkingSlot), Times.Once);
        }
        #endregion
    }
}
