using ParkingZoneApp.Enums;
using System.ComponentModel.DataAnnotations;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;

namespace ParkingSlotsTest.ModelTests
{
    public class CreateVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] {1, true, SlotCategoryEnum.Standart, 1, true},
                new object[] {2, false, SlotCategoryEnum.Business, 2, true}
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingCreateVM_ThenValidationIsPerformed(int Number, bool IsAvailableForBooking, SlotCategoryEnum Category,int ParkingZoneId, bool expectedValidation)
        {
            CreateVM createVM = new()
            {
                Number = Number,
                IsAvailableForBooking = IsAvailableForBooking,
                Category = Category,
                ParkingZoneId = ParkingZoneId
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
