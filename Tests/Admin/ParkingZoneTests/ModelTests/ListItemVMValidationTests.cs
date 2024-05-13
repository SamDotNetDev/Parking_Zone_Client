using ParkingZoneApp.ViewModels.ParkingZonesVMs;
using System.ComponentModel.DataAnnotations;

namespace Tests.Admin.ParkingZoneTests.ModelTests
{
    public class ListItemVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] {2, null, "2TestAddress", false},
                new object[] {3, "3TestName", null, false },
                new object[] {4, "4TestName", "4TestAddress", true }
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingListItemVM_ThenValidationIsPerformed(int Id, string Name, string Address, bool expectedValidation)
        {
            //Arrange
            ListItemVM listItemVM = new()
            {
                Id = Id,
                Name = Name,
                Address = Address
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
