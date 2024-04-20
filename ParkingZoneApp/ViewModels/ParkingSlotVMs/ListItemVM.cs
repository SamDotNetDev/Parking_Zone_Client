using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingSlotsVMs
{
    public class ListItemVM
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int? Number { get; set; }

        [Required]
        public bool IsAvilableForBooking { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string FeePerHour { get; set; }

        public ListItemVM() { }

        public ListItemVM(ParkingSlots parkingSlots)
        {
            Id = parkingSlots.Id;
            Number = parkingSlots.Number;
            IsAvilableForBooking = parkingSlots.IsAvilableForBooking;
            Category = parkingSlots.Category;
            FeePerHour = parkingSlots.FeePerHour;

        }
    }
}
