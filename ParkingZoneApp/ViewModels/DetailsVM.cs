
using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels
{
    public class DetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public DetailsVM MapToModel(ParkingZone parkingZone)
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
