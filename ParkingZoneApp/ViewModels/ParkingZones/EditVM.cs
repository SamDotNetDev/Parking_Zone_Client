using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class EditVM
    {

        [Required]
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime? DateOfEstablishment { get; set; }

        public EditVM(){ }

        public EditVM(ParkingZone parkingZone)
        {
            Id = parkingZone.Id;
            Name = parkingZone.Name;
            Address = parkingZone.Address;
            DateOfEstablishment = parkingZone.DateOfEstablishment;   
        }

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
    }
}
