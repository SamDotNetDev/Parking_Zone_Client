using Microsoft.AspNetCore.Mvc.Rendering;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;
using ParkingZoneApp.ViewModels.Reservation;
using System.ComponentModel.DataAnnotations;

namespace ReservationTests.ModelTests
{
    public class FreeSlotsVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
        new List<object[]>
        {
            new object[] { DateTime.Now, 1, 1, null, null, true },
        };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingFreeSlotsVM_ThenValidationIsPerformed(DateTime StartTime, int Duration, int ParkingZoneId, SelectList ParkingZones, IEnumerable<ListItemVM> ParkingSlots, bool expectedValidationResult)
        {
            //Arrange
            FreeSlotsVM freeSlotsVM = new()
            {
                StartTime = StartTime,
                Duration = Duration,
                ParkingZoneId = ParkingZoneId,
                ParkingZones = ParkingZones,
                ParkingSlots = ParkingSlots
            };
            var validationContext = new ValidationContext(freeSlotsVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var result = Validator.TryValidateObject(freeSlotsVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidationResult, result);
        }
    }
}
