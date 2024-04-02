using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels
{
    public class CreateVM
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public Models.ParkingZone MapToModel()
        {
            return new Models.ParkingZone
            {
                Name = this.Name,
                Address = this.Address,
            };
        }
    }
}