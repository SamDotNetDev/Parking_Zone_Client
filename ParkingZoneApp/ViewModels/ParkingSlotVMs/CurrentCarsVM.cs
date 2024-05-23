using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingSlotVMs
{
    public class CurrentCarsVM
    {
        public int SlotNumber { get; set; }

        public string VehicleNumber { get; set; }

        public CurrentCarsVM(Reservation reservation)
        {
            SlotNumber = reservation.ParkingSlot.Number;
            VehicleNumber = reservation.VehicleNumber;
        }
    }
}
