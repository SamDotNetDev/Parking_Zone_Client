using ParkingZoneApp.ViewModels.ReservationVMs;
using System.ComponentModel.DataAnnotations;

namespace Tests.User.ReservationTests.Model
{
    public class ReservationHistoryListItemVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] {1, DateTime.Now, 1, null, 1, null, null, true, false},
                new object[] {1, DateTime.Now, 1, "TestAddress", 1, null, null, false, false},
                new object[] {1, DateTime.Now, 1, "TestAddress", 1, "70ZZ777Z", null, true, false},
                new object[] {1, DateTime.Now, 1, "TestAddress", 1, "70ZZ777Z", "wjer-2frq32ff-wvseve-3feved", false, true},
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingReservationHistoryListItemVM_ThenValidationIsPerformed(int id, DateTime startTime, int duration, string zoneAddress, int slotNumber, string vehicleNumber, string userId, bool isActive, bool expectedValidation)
        {
            //Arrange
            ReservationHistoryListItemVM reservationHistoryVM = new()
            {
                Id = id,
                StartTime = startTime,
                Duration = duration,
                ParkingZoneAddress = zoneAddress,
                ParkingSlotNumber = slotNumber,
                VehicleNumber = vehicleNumber,
                UserId = userId,
                IsActive = isActive
            };

            var validationContext = new ValidationContext(reservationHistoryVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var result = Validator.TryValidateObject(reservationHistoryVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidation, result);
        }
    }
}
