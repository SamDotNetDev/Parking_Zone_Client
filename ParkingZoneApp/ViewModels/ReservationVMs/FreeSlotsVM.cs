using Microsoft.AspNetCore.Mvc.Rendering;
using ParkingZoneApp.Models;
using ParkingZoneApp.ViewModels.ParkingSlotVMs;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ReservationVMs
{
    public class FreeSlotsVM
    {
        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be higher than 0")]
        public int Duration { get; set; }

        [Required]
        [Display(Name = "Parking Zone")]
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
