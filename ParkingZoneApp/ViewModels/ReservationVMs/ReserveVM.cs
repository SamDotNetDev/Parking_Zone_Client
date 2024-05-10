using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ReservationVMs
{
    public class ReserveVM
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int ParkingSlotId { get; set; }

        [Required]
        public int ParkingSlotNumber { get; set; }

        public ParkingSlot ParkingSlot { get; set; }

        [Required]
        public string ParkingZoneName { get; set;}

        [Required]
        public string ParkingZoneAddress { get; set;}

        [Required]
        public string VehicleNumber { get; set;}

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Reservation MapToModel()
        {
            return new Reservation()
            {
                StartTime = this.StartTime,
                Duration = this.Duration,
                ParkingSlotId = this.ParkingSlotId,
                VehicleNumber = this.VehicleNumber
            };
        }

        public ReserveVM(ParkingSlot parkingSlot, DateTime startTime, int duration)
        {
            StartTime = startTime;
            Duration = duration;
            ParkingSlotId = parkingSlot.Id;
            ParkingZoneAddress = parkingSlot.ParkingZone.Address;
            ParkingZoneName = parkingSlot.ParkingZone.Name;
            ParkingSlotNumber = parkingSlot.Number;
        }
        public ReserveVM() { }
    }
}
