using ParkingZoneApp.ViewModels.ReservationVMs;
using System.ComponentModel.DataAnnotations;

namespace Tests.User.ReservationTests.Models
{
    public class ProlongVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] {1, 1, null, DateTime.Now, 1, false},
                new object[] {1, 1, "TestAddress", DateTime.Now, 1, true}
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingProlongVM_ThenValidationIsPerformed(int reservationId, int slotNumber, string zoneAddress, DateTime endTime, int addHours, bool expectedValidation)
        {
            //Arrange
            ProlongVM prolongVM = new()
            {
                ReservationId = reservationId,
                ParkingSlotNumber = slotNumber,
                ParkingZoneAddress = zoneAddress,
                EndTime = endTime,
                AddHours = addHours
            };

            var validationContext = new ValidationContext(prolongVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var result = Validator.TryValidateObject(prolongVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidation, result);
        }
    }
}