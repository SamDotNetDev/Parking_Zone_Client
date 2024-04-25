using Moq;
using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using ParkingZoneApp.Repositories;
using ParkingZoneApp.Services;
using System.Text.Json;
using Xunit.Sdk;

namespace ParkingSlotsTest.Services
{
    public class ParkingSlotServiceTests
    {
        private readonly Mock<IParkingSlotsRepository> _repository;
        private readonly IParkingSlotService _service;
        private readonly ParkingSlot _ParkingSlotsTest;
        private readonly int Id = 1;

        public ParkingSlotServiceTests()
        {
            _repository = new Mock<IParkingSlotsRepository>();
            _service = new ParkingSlotService(_repository.Object);
            _ParkingSlotsTest = new()
            {
                Id = Id,
                Number = 1,
                IsAvailableForBooking = true,
                Category = SlotCategoryEnum.Standart,
                ParkingZoneId = 1
            };
        }

        [Fact]
        public void GivenParkingSlotsModel_WhenInsertIsCalled_ThenRepositoryInsertIsCalled()
        {
            //Arrange
            _repository.Setup(x => x.Insert(_ParkingSlotsTest));

            //Act
            _service.Insert(_ParkingSlotsTest);

            //Assert
            _repository.Verify(x=>x.Insert(_ParkingSlotsTest), Times.Once);
        }

        [Fact]
        public void GivenParkingSlotsModel_WhenUpdateIsCalled_ThenRepositoryUptadeIsCalled()
        {
            //Arrange
            _repository.Setup(x => x.Update(_ParkingSlotsTest));

            //Act
            _service.Update(_ParkingSlotsTest);

            //Assert
            _repository.Verify(x=>x.Update(_ParkingSlotsTest), Times.Once);
        }

        [Fact]
        public void GivenParkingSlotsModel_WhenDeleteIsCalled_ThenRepositoryDeleteIsCalled()
        {
            //Arrange
            _repository.Setup(x => x.Delete(_ParkingSlotsTest));

            //Act
            _service.Delete(_ParkingSlotsTest);

            //Assert
            _repository.Verify(x => x.Delete(_ParkingSlotsTest), Times.Once);
        }

        [Fact]
        public void GivenNothing_WhenGetAllIsCalled_ThenRepositoryGetAllIsCalled()
        {
            //Arrange
            var parkingSlots = new List<ParkingSlot>();
            _repository.Setup(x => x.GetAll()).Returns(parkingSlots);

            //Act
            var result = _service.GetAll();

            //Assert
            Assert.IsAssignableFrom<IEnumerable<ParkingSlot>>(result);
            _repository.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public void GivenParkingSlotsId_WhenGetByIdIsCalled_ThenRepositoryGetByIdIsCalled()
        {
            //Arrange
            _repository.Setup(x => x.GetById(Id)).Returns(_ParkingSlotsTest);

            //Act
            var result = _service.GetById(Id);

            //Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ParkingSlot>(result);
            _repository.Verify(x => x.GetById(Id), Times.Once);
            Assert.Equal(JsonSerializer.Serialize(_ParkingSlotsTest), JsonSerializer.Serialize(model));
        }

        [Fact]
        public void GivenParkingZoneId_WhenGetByParkingZoneIdIsCalled_ThenReturnsParkingSlots()
        {
            var ParkingSlots = new List<ParkingSlot>() { _ParkingSlotsTest };
            _repository.Setup(x => x.GetAll()).Returns(ParkingSlots);

            var result = _service.GetByParkingZoneId(Id);

            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ParkingSlot>>(result);
            _repository.Verify(x => x.GetAll(), Times.Once);
            Assert.Equal(JsonSerializer.Serialize(ParkingSlots), JsonSerializer.Serialize(model));
        }

        [Fact]
        public void GivenParkingZoneIdAndParkingSlotNumber_WhenParkingSlotExistsIsCalled_ThenReturnsTrue()
        {
            var ParkingSlots = new List<ParkingSlot>() { _ParkingSlotsTest};
            _repository.Setup(x => x.GetAll()).Returns(ParkingSlots);

            var result = _service.ParkingSlotExits(_ParkingSlotsTest.ParkingZoneId, _ParkingSlotsTest.Number);

            Assert.IsType<Boolean>(result);
            Assert.True(result);
            _repository.Verify(x => x.GetAll(), Times.Once);
        }
        
        [Fact]
        public void GivenParkingZoneIdAndParkingSlotNumber_WhenParkingSlotExistsIsCalled_ThenReturnsFalse()
        {
            var ParkingSlots = new List<ParkingSlot>();
            _repository.Setup(x => x.GetAll()).Returns(ParkingSlots);

            var result = _service.ParkingSlotExits(_ParkingSlotsTest.ParkingZoneId, _ParkingSlotsTest.Number);

            Assert.IsType<Boolean>(result);
            Assert.False(result);
            _repository.Verify(x => x.GetAll(), Times.Once);
        }
    }
}
