using ParkingZoneApp.ViewModels.ParkingZonesVMs;
using System.ComponentModel.DataAnnotations;

namespace Tests.Admin.ParkingZoneTests.ModelTests
{
    public class CreateVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
        new List<object[]>
        {
            new object[] { "Test2", null, false },
            new object[] { null, "Test2", false },
            new object[] { "Test3", "Test Address3", true }
        };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingCreateVM_ThenValidationIsPerformed(string Name, string Address, bool expectedValidationResult)
        {
            //Arrange
            CreateVM createVM = new()
            {
                Name = Name,
                Address = Address
            };
            var validationContext = new ValidationContext(createVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var result = Validator.TryValidateObject(createVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidationResult, result);
        }
    }
}
