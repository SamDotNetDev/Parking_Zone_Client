using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;

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
                IsAvilableForBooking = IsAvailableForBooking,
                Category = Category,
                ParkingZoneId = ParkingZoneId
            };
        }
    }
}
