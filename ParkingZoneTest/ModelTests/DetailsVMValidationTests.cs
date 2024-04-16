using ParkingZoneApp.ViewModels.ParkingZones;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneTest.ModelTests
{
    public class DetailsVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] {null, "TestName", "TestAddress"},
                new object[] {2, null, "2TestAddress"},
                new object[] {3, "3TestName", null }
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenInvalidData_WhenCreatingDetailsVM_ThenValidationIsFalse(int? Id, string Name, string Address)
        {
            //Arrange
            DetailsVM detailsVM = new()
            {
                Id = Id,
                Name = Name,
                Address = Address
            };

            var validationContext = new ValidationContext(detailsVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var result = Validator.TryValidateObject(detailsVM, validationContext,validationResult);

            //Assert
            Assert.NotEmpty(validationResult);
            Assert.False(result);
        }
    }
}
