using ParkingZoneApp.ViewModels.ParkingZonesVMs;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneTest.ModelTests
{
    public class DetailsVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] { 2, null, "2TestAddress", DateTime.Now, false },
                new object[] { 3, "3TestName", null, DateTime.Now, false },
                new object[] { 4, "4TestName", "4TestAddress", DateTime.Now, true }
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingDetailsVM_ThenValidationIsPerformed(int Id, string Name, string Address, DateTime Date, bool expectedValidation)
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
            Assert.Equal(expectedValidation, result);
        }
    }
}
