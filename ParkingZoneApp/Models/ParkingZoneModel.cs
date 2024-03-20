using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Models
{
    public class ParkingZoneModel
    {
        [Key]
        [Required]
        public int ParkingZoneId { get; set; }

        [Required]
        public string ParkingZoneName { get; set; }

        [Required]
        public string Adress { get; set; }

        [Required]
        public DateTime ParkingZoneDateOfEstablishment { get; set; }

    }
}
