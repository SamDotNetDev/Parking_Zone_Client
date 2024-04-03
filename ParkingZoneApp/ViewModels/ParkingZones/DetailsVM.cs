using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class DetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public DetailsVM MapToVM(ParkingZone parkingZone)
        {
            return new DetailsVM
            {
                Id = parkingZone.Id,
                Name = parkingZone.Name,
                Address = parkingZone.Address
            };
        }
    }
}
