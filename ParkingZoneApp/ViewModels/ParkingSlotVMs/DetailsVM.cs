using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingSlotVMs
{
    public class DetailsVM
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public bool IsAvailableForBooking { get; set; }

        [Required]
        public SlotCategoryEnum Category { get; set; }

        public DetailsVM() { }

        public DetailsVM(ParkingSlot parkingSlot)
        {
            Id = parkingSlot.Id;
            Number = parkingSlot.Number;
            IsAvailableForBooking = parkingSlot.IsAvailableForBooking;
            Category = parkingSlot.Category;
        }
    }
}
