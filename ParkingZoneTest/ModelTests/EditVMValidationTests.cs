using ParkingZoneApp.ViewModels.ParkingZones;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneTest.ModelTests
{
    public class EditVMValidationTests
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
        public void GivenInvalidItem_WhenCreatingEditVM_ThenValidationIsFalse(int? Id, string Name, string Address, DateTime? Date)
        {
            //Arrange
            EditVM editVM = new()
            {
                Id = Id,
                Name = Name, 
                Address = Address,
                DateOfEstablishment = Date
            };

            var validationContext = new ValidationContext(editVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var Result = Validator.TryValidateObject(editVM, validationContext, validationResult);

            //Assert
            Assert.False(Result);
            Assert.NotEmpty(validationResult);
        }
    }
}
