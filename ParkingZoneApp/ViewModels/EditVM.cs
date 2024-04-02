
using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels
{
    public class EditVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateOfEstablishment {  get; set; }

        public EditVM MapToModel(ParkingZone parkingZone)
        {
            return new EditVM
            {
                Id = parkingZone.Id,
                Name = parkingZone.Name,
                Address = parkingZone.Address,
                DateOfEstablishment = parkingZone.DateOfEstablishment
            };
        }
    }
}
