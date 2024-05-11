using Moq;
using ParkingZoneApp.Services;
using ParkingZoneApp.Repositories;
using ParkingZoneApp.Models;
using System.Text.Json;

namespace Tests.Admin.ParkingZoneTests.Services
{
    public class ParkingZoneServiceTests
    {
        private readonly Mock<IParkingZoneRepository> _repository;
        private readonly IParkingZoneService _service;
        private readonly ParkingZone _ParkingZoneTest;
        private readonly int Id = 1;

        public ParkingZoneServiceTests()
        {
            _repository = new Mock<IParkingZoneRepository>();
            _service = new ParkingZoneService(_repository.Object);
            _ParkingZoneTest = new()
            {
                Id = 1,
                Name = "Test",
                Address = "Test Address",
                DateOfEstablishment = DateTime.Now
            };
        }

        #region Insert
        [Fact]
        public void GivenParkingZoneModel_WhenInsertIsCalled_ThenRepositoryInsertIsCalled()
        {
            // Arrange
            _repository.Setup(repo => repo.Insert(_ParkingZoneTest));

            // Act
            _service.Insert(_ParkingZoneTest);

            // Assert
            _repository.Verify(x => x.Insert(_ParkingZoneTest), Times.Once());
        }
        #endregion

        #region Update
        [Fact]
        public void GivenParkingZoneModel_WhenUpdateIsCalled_ThenRepositoryUpdateIsCalled()
        {
            // Arrange
            _repository.Setup(repo => repo.Update(_ParkingZoneTest));

            // Act
            _service.Update(_ParkingZoneTest);

            // Assert
            _repository.Verify(x => x.Update(_ParkingZoneTest), Times.Once());
        }
        #endregion

        #region Delete
        [Fact]
        public void GivenParkingZoneModel_WhenDeleteIsCalled_ThenRepositoryDeleteIsCalled()
        {
            // Arrange
            _repository.Setup(repo => repo.Delete(_ParkingZoneTest));

            // Act
            _service.Delete(_ParkingZoneTest);

            // Assert
            _repository.Verify(x => x.Delete(_ParkingZoneTest), Times.Once());
        }
        #endregion

        #region GetAll
        [Fact]
        public void GivenNothing_WhenGetAllIsCalled_ThenRepositoryGetAllIsCalled()
        {
            // Arrange
            var parkingZones = new List<ParkingZone>();
            _repository.Setup(repo => repo.GetAll()).Returns(parkingZones);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<ParkingZone>>(result);
            _repository.Verify(x => x.GetAll(), Times.Once());
        }
        #endregion

        #region GetById
        [Fact]
        public void GivenParkingZoneId_WhenGetByIdIsCalled_ThenRepositoryGetByIdIsCalled()
        {
            // Arrange
            _repository.Setup(repo => repo.GetById(Id)).Returns(_ParkingZoneTest);

            // Act
            var result = _service.GetById(Id);

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ParkingZone>(result);
            _repository.Verify(x => x.GetById(Id), Times.Once());
            Assert.Equal(JsonSerializer.Serialize(_ParkingZoneTest), JsonSerializer.Serialize(model));
        }
        #endregion
    }
}