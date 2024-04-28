using Moq;
using System.Text.Json;
using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services;
using Microsoft.AspNetCore.Mvc;
using ParkingZoneApp.Areas.Admin;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;

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
                Category = SlotCategoryEnum.Standart,
                IsAvailableForBooking = true,
                ParkingZoneId = 1
            };
        }

        #region Index
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
            CreateVM createVM = new() { ParkingZoneId = 1, Number = -1};
            _slotService.Setup(x => x.ParkingSlotExits(createVM.ParkingZoneId, createVM.Number)).Returns(false);

            //Act
            var result = _controller.Create(createVM);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<CreateVM>(model);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _slotService.Verify(x => x.ParkingSlotExits(createVM.ParkingZoneId, createVM.Number), Times.Once);
        }
        
        [Fact]
        public void GivenCreateVM_WhenCreateIsCalledToPost_ThenSlotExistsAndModelStateIsFalseAndReturnsRedirectToAction()
        {
            //Arrange
            CreateVM createVM = new() { ParkingZoneId = 1, Number = 1};
            _slotService.Setup(x => x.ParkingSlotExits(createVM.ParkingZoneId, createVM.Number)).Returns(true);

            //Act
            var result = _controller.Create(createVM);

            //Assert
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<CreateVM>(model);
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            _slotService.Verify(x => x.ParkingSlotExits(createVM.ParkingZoneId, createVM.Number), Times.Once);
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
            _slotService.Setup(x => x.ParkingSlotExits(createVM.ParkingZoneId, createVM.Number)).Returns(false);
            _slotService.Setup(x => x.Insert(It.IsAny<ParkingSlot>()));

            //Act
            var result = _controller.Create(createVM);

            //Assert
            var action = Assert.IsType<RedirectToActionResult>(result).ActionName;
            Assert.Equal("Index", action);
            Assert.NotNull(result);
            Assert.True(_controller.ModelState.IsValid);
            _slotService.Verify(x => x.ParkingSlotExits(createVM.ParkingZoneId, createVM.Number), Times.Once);
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
                ParkingZoneId = 1
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
            EditVM editVM = new() { Id = 2 };

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
            EditVM editVM = new() { Id = Id };
            _controller.ModelState.AddModelError("field", "property is required");

            //Act
            var result = _controller.Edit(Id, editVM);

            //Assert
            Assert.False(_controller.ModelState.IsValid);
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<EditVM>(model);
            Assert.Equal(JsonSerializer.Serialize(editVM), JsonSerializer.Serialize(model));
        }
        
        [Fact]
        public void GivenParkingSlotIdAndEditVM_WhenEditIsCalledToPost_ThenNumberIsNegativeAndModelStateIsFalseAndReturnsViewResult()
        {
            //Arrange
            EditVM editVM = new() { Id = Id, Number = -1, ParkingZoneId = 1 };
            _slotService.Setup(x => x.GetById(Id)).Returns(_parkingSlotsTest);
            _slotService.Setup(x => x.ParkingSlotExits(editVM.ParkingZoneId, editVM.Number)).Returns(false);
            //Act
            var result = _controller.Edit(Id, editVM);

            //Assert
            Assert.False(_controller.ModelState.IsValid);
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<EditVM>(model);
            Assert.Equal(JsonSerializer.Serialize(editVM), JsonSerializer.Serialize(model));
            _slotService.Verify(x => x.GetById(Id), Times.Once());
            _slotService.Verify(x => x.ParkingSlotExits(editVM.ParkingZoneId, editVM.Number), Times.Once);
        }

        [Fact]
        public void GivenParkingSlotIdAndEditVM_WhenEditIsCalledToPost_ThenParkingSlotExistsAndNumerIsNotSameAndModelStateIsFalseAndReturnsViewResult()
        {
            //Arrange
            EditVM editVM = new() { Id = Id, Number = 2, ParkingZoneId = 1 };
            _slotService.Setup(x => x.GetById(Id)).Returns(_parkingSlotsTest);
            _slotService.Setup(x => x.ParkingSlotExits(editVM.ParkingZoneId, editVM.Number)).Returns(true);
            //Act
            var result = _controller.Edit(Id, editVM);

            //Assert
            Assert.False(_controller.ModelState.IsValid);
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<EditVM>(model);
            Assert.Equal(JsonSerializer.Serialize(editVM), JsonSerializer.Serialize(model));
            _slotService.Verify(x => x.GetById(Id), Times.Once());
            _slotService.Verify(x => x.ParkingSlotExits(editVM.ParkingZoneId, editVM.Number), Times.Once);
        }
        
        [Fact]
        public void GivenParkingSlotIdAndEditVM_WhenEditIsCalledToPost_ThenParkingSlotModelStateIsTrueAndReturnsRedirectToAction()
        {
            //Arrange
            EditVM editVM = new() { Id = Id, Number = 1, ParkingZoneId = 1 };
            _slotService.Setup(x => x.GetById(Id)).Returns(_parkingSlotsTest);
            _slotService.Setup(x => x.ParkingSlotExits(editVM.ParkingZoneId, editVM.Number)).Returns(false);
            _slotService.Setup(x => x.Update(_parkingSlotsTest));
            //Act
            var result = _controller.Edit(Id, editVM);

            //Assert
            Assert.True(_controller.ModelState.IsValid);
            Assert.NotNull(result);
            var action = Assert.IsType<RedirectToActionResult>(result).ActionName;
            Assert.Equal("Index", action);
            _slotService.Verify(x => x.GetById(Id), Times.Once());
            _slotService.Verify(x => x.ParkingSlotExits(editVM.ParkingZoneId, editVM.Number), Times.Once);
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
    }
}
