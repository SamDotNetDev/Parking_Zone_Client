using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingZoneApp.Models
{
    [Table("ParkingSlots")]
    public class ParkingSlots
    {
        [Key]
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

        [Required]
        public int ParkingZoneId { get; set; }
    }
}
