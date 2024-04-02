
using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels
{
    public class DeleteVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateOfEstablishment { get; set; }

        public DeleteVM MapToModel(ParkingZone parkingZone)
        {
            return new DeleteVM
            {
                Id = parkingZone.Id,
                Name = parkingZone.Name,
                Address = parkingZone.Address,
                DateOfEstablishment = parkingZone.DateOfEstablishment,
            };
        }
    }
}
