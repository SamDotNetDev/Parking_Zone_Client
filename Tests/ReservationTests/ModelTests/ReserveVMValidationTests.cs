using Microsoft.AspNetCore.Mvc.Rendering;
using ParkingZoneApp.Models;
using ParkingZoneApp.ViewModels.ReservationVMs;
using System;
using System.ComponentModel.DataAnnotations;

namespace Tests.ReservationTests.ModelTests
{
    public class ReserveVMValidationTests
    {
        public static IEnumerable<object[]> TestData =>
        new List<object[]>
        {
            new object[] { 1, DateTime.Now, 1, 1, 1, null, "TestName", "TestAddress", "TestVehicle", "TestUserId", null, true },
            new object[] { 1, DateTime.Now, 1, 1, 1, null, "TestName", "TestAddress", "TestVehicle", null, null, true },
            new object[] { 1, DateTime.Now, 1, 1, 1, null, null, "TestAddress", "TestVehicle", "TestUserId", null, false },
            new object[] { 1, DateTime.Now, 1, 1, 1, null, "TestName", null, "TestVehicle", "TestUserId", null, false },
            new object[] { 1, DateTime.Now, 1, 1, 1, null, "TestName", "TestAddress", null, "TestUserId", null, false },
        };

        [Theory]
        [MemberData(nameof(TestData))]
        public void GivenItemToBeValidated_WhenCreatingReserveVM_ThenValidationIsPerformed(int id, DateTime startTime, int duration, int parkingSlotId, int parkingSlotNumber, ParkingSlot parkingSlot, string parkingZoneName, string parkingZoneAddress, string vehicleNumber, string userId, ApplicationUser user, bool expectedValidationResult)
        {
            //Arrange
            ReserveVM reserveVM = new()
            {
                Id = id,
                StartTime = startTime,
                Duration = duration,
                ParkingSlotId = parkingSlotId,
                ParkingSlotNumber = parkingSlotNumber,
                ParkingSlot = parkingSlot,
                ParkingZoneName = parkingZoneName,
                ParkingZoneAddress = parkingZoneAddress,
                VehicleNumber = vehicleNumber,
                UserId = userId,
                User = user
            };
            var validationContext = new ValidationContext(reserveVM, null, null);
            var validationResult = new List<ValidationResult>();

            //Act
            var result = Validator.TryValidateObject(reserveVM, validationContext, validationResult);

            //Assert
            Assert.Equal(expectedValidationResult, result);
        }
    }
}
