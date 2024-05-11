using ParkingZoneApp.Enums;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;
using System.ComponentModel.DataAnnotations;

namespace Tests.Admin.ParkingSlotTests.ModelTests
{
    public class DetailsVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] {1, 1, true, SlotCategoryEnum.Standart, true},
                new object[] {2, 2, false, SlotCategoryEnum.Business, true}
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingDetailsVM_ThenValidationIsPerformed(int Id, int Number, bool IsAvailableForBooking, SlotCategoryEnum Category, bool expectedValidation)
        {
            //Arrange
            DetailsVM detailsVM = new()
            {
                Id = Id,
                Number = Number,
                IsAvailableForBooking = IsAvailableForBooking,
                Category = Category,
            };

            var validationContext = new ValidationContext(detailsVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var result = Validator.TryValidateObject(detailsVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidation, result);
        }
    }
}
