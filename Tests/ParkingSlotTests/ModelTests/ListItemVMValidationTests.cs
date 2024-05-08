using ParkingZoneApp.Enums;
using System.ComponentModel.DataAnnotations;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;

namespace ParkingSlotsTest.ModelTests
{
    public class ListItemVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] {3, 2, true, SlotCategoryEnum.Standart, true},
                new object[] {3, 2, false, SlotCategoryEnum.Business, true}
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingListItemVM_ThenValidationIsPerformed(int Id, int Number, bool IsAvailableForBooking, SlotCategoryEnum Category, bool expectedValidation)
        {
            //Arrange
            ListItemVM listItemVM = new()
            {
                Id = Id,
                Number = Number,
                IsAvailableForBooking = IsAvailableForBooking,
                Category = Category
            };

            var validationContext = new ValidationContext(listItemVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var result = Validator.TryValidateObject(listItemVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidation, result);
        }
    }
}
