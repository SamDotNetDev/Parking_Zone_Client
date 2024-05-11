using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ReservationVMs
{
    public class ReservationHistoryListVM
    {        
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public string ParkingZoneAddress { get; set; }

        [Required]
        public int ParkingSlotNumber { get; set; }

        [Required]
        public string VehicleNumber { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ReservationHistoryListVM(Reservation reservation)
        {
            Id = reservation.Id;
            UserId = reservation.UserId;
            StartTime = reservation.StartTime;
            Duration = reservation.Duration;
            ParkingZoneAddress = reservation.ParkingSlot.ParkingZone.Address;
            ParkingSlotNumber = reservation.ParkingSlot.Number;
            VehicleNumber = reservation.VehicleNumber;
            IsActive = reservation.IsActive;
        }

        public ReservationHistoryListVM() { }
    }
}