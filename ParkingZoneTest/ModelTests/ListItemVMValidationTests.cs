using ParkingZoneApp.ViewModels.ParkingZones;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneTest.ModelTests
{
    public class ListItemVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] {null, "TestName", "TestAddress", DateTime.Now},
                new object[] {2, null, "2TestAddress", DateTime.Now},
                new object[] {3, "3TestName", null, DateTime.Now },
                new object[] {4, "4TestName", "4TestAddress", null }
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenInvalidItem_WhenCreatingListItemVM_ThenValidationIsFalse(int? Id, string Name, string Address, DateTime? Date)
        {
            //Arrange
            ListItemVM listItemVM = new()
            {
                Id = Id,
                Name = Name,
                Address = Address,
                DateOfEstablishment = Date
            };

            var validationContext = new ValidationContext(listItemVM,null,null);
            var validationResult = new List<ValidationResult>();

            //Act
            var result = Validator.TryValidateObject(listItemVM, validationContext,validationResult);

            //Assert
            Assert.False(result);
            Assert.NotEmpty(validationResult);
        }
    }
}
