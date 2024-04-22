using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingZonesVMs
{
    public class DetailsVM
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime DateOfEstablishment { get; set; }

        public DetailsVM() { }

        public DetailsVM (ParkingZone parkingZone)
        {
            Id = parkingZone.Id;
            Name = parkingZone.Name;
            Address = parkingZone.Address;
            DateOfEstablishment = parkingZone.DateOfEstablishment;
        }
    }
}
