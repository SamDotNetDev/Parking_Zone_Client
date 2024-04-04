using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            
        }
        public DetailsVM (ParkingZone parkingZone)
        {
            Id = parkingZone.Id;
            Name = parkingZone.Name;
            Address = parkingZone.Address;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

    }
}
