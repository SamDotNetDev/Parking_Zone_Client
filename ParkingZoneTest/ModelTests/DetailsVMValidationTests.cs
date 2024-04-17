using ParkingZoneApp.ViewModels.ParkingZones;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneTest.ModelTests
{
    public class DetailsVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] { null, "TestName", "TestAddress", DateTime.Now },
                new object[] { 2, null, "2TestAddress", DateTime.Now },
                new object[] { 3, "3TestName", null, DateTime.Now },
                new object[] { 4, "4TestName", "4TestAddress", null }
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenInvalidData_WhenCreatingDetailsVM_ThenValidationIsFalse(int? Id, string Name, string Address, DateTime? Date)
        {
            //Arrange
            DetailsVM detailsVM = new()
            {
                Id = Id,
                Name = Name,
                Address = Address,
                DateOfEstablishment = Date
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
