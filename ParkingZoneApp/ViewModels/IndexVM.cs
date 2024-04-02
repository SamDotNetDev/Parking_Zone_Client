using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels
{
    public class IndexVM
    {
        public IEnumerable<ParkingZone> ParkingZones { get; set; }

        public IndexVM MapToModel(IEnumerable<ParkingZone> parkingZones)
        {
            return new IndexVM
            {
                ParkingZones = parkingZones
            };
        }
    }
}