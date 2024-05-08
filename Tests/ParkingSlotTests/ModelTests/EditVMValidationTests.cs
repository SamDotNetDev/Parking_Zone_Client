using ParkingZoneApp.Enums;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;
using System.ComponentModel.DataAnnotations;

namespace ParkingSlotsTest.ModelTests
{
    public class EditVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] {1, 1, true, SlotCategoryEnum.Standart, 1, true},
                new object[] {2, 2, false, SlotCategoryEnum.Business, 2, true}
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingCreateVM_ThenValidationIsPerformed(int Id, int Number, bool IsAvailableForBooking, SlotCategoryEnum Category, int ParkingZoneId, bool expectedValidation)
        {
            //Arrange
            EditVM editVM = new()
            {
                Id = Id,
                Number = Number,
                IsAvailableForBooking = IsAvailableForBooking,
                Category = Category,
                ParkingZoneId = ParkingZoneId
            };

            var validationContext = new ValidationContext(editVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var result = Validator.TryValidateObject(editVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidation, result);
        }
    }
}
