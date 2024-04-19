using ParkingZoneApp.ViewModels.ParkingZones;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneTest.ModelTests
{
    public class EditVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] { 2, null, "2TestAddress", false },
                new object[] { 3, "3TestName", null, false },
                new object[] { 4, "4TestName", "4TestAddress", true }
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingEditVM_ThenValidationIsPerformed(int Id, string Name, string Address, bool expectedValidationResult)
        {
            //Arrange
            EditVM editVM = new()
            {
                Id = Id,
                Name = Name, 
                Address = Address
            };

            var validationContext = new ValidationContext(editVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var result = Validator.TryValidateObject(editVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidationResult, result);
        }
    }
}
