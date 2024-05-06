using ParkingZoneApp.Data.Migrations;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Models
{
    public class Reservation
    {
        [Key]
        [Required]
        public int ReservationId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int ParkingSlotId { get; set; }

        [Required]
        public virtual ParkingSlot ParkingSlot { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}
