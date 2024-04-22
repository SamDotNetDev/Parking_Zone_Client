using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingZoneApp.Models
{
    [Table("ParkingZone")]
    public class ParkingZone
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime DateOfEstablishment { get; set; }

        [Required]
        public ICollection<ParkingSlot> ParkingSlots { get; set; }

    }
}
