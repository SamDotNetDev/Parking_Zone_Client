using Moq;
using ParkingZoneApp.Enums.ParkingSlotCategories;
using ParkingZoneApp.Models;
using ParkingZoneApp.Repositories;
using ParkingZoneApp.Services;
using System.Text.Json;

namespace ParkingSlotsTest.Services
{
    public class ParkingSlotsServiceTests
    {
        private readonly Mock<IParkingSlotsRepository> _repository;
        private readonly IParkingSlotsService _service;
        private readonly ParkingSlots _ParkingSlotsTest;
        private readonly int Id = 1;

        public ParkingSlotsServiceTests()
        {
            _repository = new Mock<IParkingSlotsRepository>();
            _service = new ParkingSlotsService(_repository.Object);
            _ParkingSlotsTest = new()
            {
                Id = 1,
                Number = 1,
                IsAvilableForBooking = true,
                Category = Categories.Econom.ToString(),
                FeePerHour = "1",
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
            var parkingSlots = new List<ParkingSlots>();
            _repository.Setup(x => x.GetAll()).Returns(parkingSlots);

            //Act
            var result = _service.GetAll();

            //Assert
            Assert.IsAssignableFrom<IEnumerable<ParkingSlots>>(result);
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
            var model = Assert.IsType<ParkingSlots>(result);
            _repository.Verify(x => x.GetById(Id), Times.Once);
            Assert.Equal(JsonSerializer.Serialize(_ParkingSlotsTest), JsonSerializer.Serialize(model));
        }
    }
}
