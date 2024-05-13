using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ReservationVMs
{
    public class ProlongVM
    {
        [Required]
        public int ReservationId { get; set; }

        [Required]
        public int ParkingSlotNumber { get; set; }

        [Required]
        public string ParkingZoneAddress { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be higher than 0")]
        public int AddHours { get; set; }

        public ProlongVM(Reservation reservation)
        {
            ReservationId = reservation.Id;
            ParkingSlotNumber = reservation.ParkingSlot.Number;
            ParkingZoneAddress = reservation.ParkingSlot.ParkingZone.Address;
            EndTime = reservation.StartTime.AddHours(reservation.Duration);
        }

        public ProlongVM() { }
    }
}