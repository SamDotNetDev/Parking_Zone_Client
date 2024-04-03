using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class ListItemVM
    {
        public IEnumerable<ParkingZone> ParkingZones { get; set; }

        public ListItemVM MapToVM(IEnumerable<ParkingZone> parkingZones)
        {
            return new ListItemVM
            {
                ParkingZones = parkingZones
            };
        }
    }
}