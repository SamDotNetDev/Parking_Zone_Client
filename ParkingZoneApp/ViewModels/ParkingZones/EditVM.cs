using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class EditVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateOfEstablishment { get; set; }

        public ParkingZone MapToModel(EditVM VM)
        {
            return new ParkingZone
            {
                Id = VM.Id,
                Name = VM.Name,
                Address = VM.Address,
                DateOfEstablishment = VM.DateOfEstablishment
            };
        }
        public EditVM MapToVM(ParkingZone parkingZone)
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
