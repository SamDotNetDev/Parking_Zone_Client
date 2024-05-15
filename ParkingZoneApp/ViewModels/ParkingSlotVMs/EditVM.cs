using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingSlotVMs
{
    public class EditVM
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Number { get; set; }

        [Required]
        public bool IsAvailableForBooking { get; set; }

        [Required]
        public SlotCategoryEnum Category { get; set; }

        [Required]
        public int ParkingZoneId { get; set; }

        [Required]
        public bool IsInUse { get; set; }

        public EditVM() { }

        public EditVM(ParkingSlot parkingSlot)
        {
            Id = parkingSlot.Id;
            Number = parkingSlot.Number;
            IsAvailableForBooking = parkingSlot.IsAvailableForBooking;
            Category = parkingSlot.Category;
            ParkingZoneId = parkingSlot.ParkingZoneId;
            IsInUse = parkingSlot.IsInUse;
        }

        public ParkingSlot MapToModel(ParkingSlot parkingslot)
        {
            parkingslot.Number = Number;
            parkingslot.IsAvailableForBooking = IsAvailableForBooking;
            parkingslot.Category = Category;
            return parkingslot;
        }
    }
}
