using ParkingZoneApp.ViewModels.ParkingSlotsVMs;
using System.ComponentModel.DataAnnotations;

namespace ParkingSlotsTest.ModelTests
{
    public class ListItemVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] {1, null, true, "Standart", "1000", false},
                new object[] {2, 1, false, null, "1000", false},
                new object[] {3, 2, true, "Business", null, false},
                new object[] {4, 3, false, "2Standart", "2000", true}
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingListItemVM_ThenValidationIsPerformed(int Id, int? Number, bool IsAvailableForBooking, string Category, string FeePerHour, bool expectedValidation)
        {
            ListItemVM listItemVM = new()
            {
                Id = Id,
                Number = Number,
                IsAvilableForBooking = IsAvailableForBooking,
                Category = Category,
                FeePerHour = FeePerHour
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
