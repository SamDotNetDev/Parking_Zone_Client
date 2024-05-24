using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingSlotVMs
{
    public class CurrentCarsVM
    {
        [Required]
        public int SlotNumber { get; set; }

        [Required]
        public string VehicleNumber { get; set; }

        public CurrentCarsVM() { }

        public CurrentCarsVM(Reservation reservation)
        {
            SlotNumber = reservation.ParkingSlot.Number;
            VehicleNumber = reservation.VehicleNumber;
        }
    }
}
