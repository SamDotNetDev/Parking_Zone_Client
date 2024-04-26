using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingSlotVMs
{
    public class CreateVM
    {
        [Required]
        public int Number { get; set; }

        [Required]
        public bool IsAvailableForBooking { get; set; }

        [Required]
        public SlotCategoryEnum Category { get; set; }

        [Required]
        public int ParkingZoneId { get; set; }

        public CreateVM() { }

        public ParkingSlot MapToModel()
        {
            return new ParkingSlot
            {
                Number = Number,
                IsAvailableForBooking = IsAvailableForBooking,
                Category = Category,
                ParkingZoneId = ParkingZoneId
            };
        }
    }
}