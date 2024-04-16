using ParkingZoneApp.ViewModels.ParkingZones;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneTest.ModelTests
{
    public class CreateVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
        new List<object[]>
        {
            new object[] { null, "Test Address" },
            new object[] { "Test2", null }
        };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenInvalidItem_WhenCreatingCreateVM_ThenValidationIsFalse(string Name, string Address)
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
            var result = Validator.TryValidateObject(createVM,validationContext,validationResult);

            //Assert
            Assert.False(result);
            Assert.NotEmpty(validationResult);
        }
    }
}
