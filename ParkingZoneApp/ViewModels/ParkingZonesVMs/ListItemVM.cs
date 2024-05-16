using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingZonesVMs
{
    public class ListItemVM
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public ICollection<ParkingSlot> ParkingSlots { get; set; }

        public ListItemVM() { }

        public ListItemVM(ParkingZone parkingZone)
        {
            Id = parkingZone.Id;
            Name = parkingZone.Name;
            Address = parkingZone.Address;
            ParkingSlots = parkingZone.ParkingSlots;
        }
    }
}