using ParkingZoneApp.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingZoneApp.Models
{
    [Table("ParkingSlots")]
    public class ParkingSlot
    {
        [Key]
        [Required] 
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public bool IsAvailableForBooking { get; set; }

        [Required]
        public SlotCategoryEnum Category { get; set; }

        [Required]
        public int ParkingZoneId { get; set; }

        [Required]
        public virtual ParkingZone ParkingZone { get; set; }

        [Required]
        public virtual ICollection<Reservation> Reservations { get; set;}
    }
}
