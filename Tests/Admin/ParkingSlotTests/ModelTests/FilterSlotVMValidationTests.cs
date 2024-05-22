using ParkingZoneApp.Enums;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;
using System.ComponentModel.DataAnnotations;

namespace Tests.Admin.ParkingSlotTests.ModelTests
{
    public class FilterSlotVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] {1, null, true, true},
                new object[] {2, SlotCategoryEnum.Business, null, true},
                new object[] {2, SlotCategoryEnum.Business, false, true}
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingDetailsVM_ThenValidationIsPerformed(int parkingZoneId, SlotCategoryEnum? category, bool? IsSlotFree, bool expectedValidation)
        {
            //Arrange
            FilterSlotVM filterSlotVM = new()
            {
                ParkingZoneId = parkingZoneId,
                Category = category,
                IsSlotFree = IsSlotFree
            };

            var validationContext = new ValidationContext(filterSlotVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var result = Validator.TryValidateObject(filterSlotVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidation, result);
        }
    }
}
