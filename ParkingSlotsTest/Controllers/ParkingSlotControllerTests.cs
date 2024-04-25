using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingZoneApp.Areas.Admin;
using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using ParkingZoneApp.Services;
using ParkingZoneApp.ViewModels.ParkingSlotsVMs;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;
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
            
            //Act
            var result = _controller.Create(Id);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ViewResult>(result).Model;
            Assert.IsType<CreateVM>(model);
        }

        [Fact]
        public void GivenCreateVM_WhenCreateIsCalledToPost_ThenModelStateIsFalseAndReturnsVM()
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
        public void GivenCreateVM_WhenCreateIsCalledToPost_ThenNumberIsNegativeAndModelStateIsFalseAndReturnsVM()
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
            Assert.IsType<RedirectToActionResult>(result);
            Assert.NotNull(result);
            Assert.True(_controller.ModelState.IsValid);
            _slotService.Verify(x => x.ParkingSlotExits(createVM.ParkingZoneId, createVM.Number), Times.Once);
            _slotService.Verify(x => x.Insert(It.IsAny<ParkingSlot>()), Times.Once);
        }
        #endregion
    }
}
