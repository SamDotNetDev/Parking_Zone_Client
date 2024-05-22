using ParkingZoneApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingSlotVMs
{
    public class FilterSlotVM
    {
        [Required]
        public int ParkingZoneId { get; set; }

        public SlotCategoryEnum? Category { get; set; } = null;

        public bool? IsSlotFree { get; set; } = null;
    }
}
