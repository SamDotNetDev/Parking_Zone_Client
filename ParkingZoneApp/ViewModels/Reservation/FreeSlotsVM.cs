using Microsoft.AspNetCore.Mvc.Rendering;
using ParkingZoneApp.Models;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.Reservation
{
    public class FreeSlotsVM
    {
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be higher than 0")]
        public int Duration { get; set; }

        [Required]
        public int ParkingZoneId { get; set; }

        public SelectList ParkingZones { get; set; }

        public IEnumerable<ListItemVM> ParkingSlots { get; set; }

        public FreeSlotsVM(IEnumerable<ParkingZone> zones)
        {
            ParkingZones = new SelectList(zones, "Id", "Name");
        }
        public FreeSlotsVM() { }
    }
}
