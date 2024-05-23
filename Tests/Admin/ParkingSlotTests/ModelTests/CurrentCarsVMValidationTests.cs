using ParkingZoneApp.Enums;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;
using System.ComponentModel.DataAnnotations;

namespace Tests.Admin.ParkingSlotTests.ModelTests
{
    public class CurrentCarsVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] {1, null, false},
                new object[] {2, "777", true}
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingCreateVM_ThenValidationIsPerformed(int slotNumber, string vehicleNumber, bool expectedValidation)
        {
            //Arrange
            CurrentCarsVM createVM = new()
            {
                SlotNumber = slotNumber,
                VehicleNumber = vehicleNumber
            };

            var validationContext = new ValidationContext(createVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var result = Validator.TryValidateObject(createVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidation, result);
        }
    }
}
