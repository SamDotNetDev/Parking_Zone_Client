using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class ListItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateOfEstablishment { get; set; }

        public IEnumerable<ListItemVM> VMs(IEnumerable<ParkingZone> parkingZones)
        {
            var listItemVMs = parkingZones.Select(parkingZone => new ListItemVM
            {
                Id = parkingZone.Id,
                Name = parkingZone.Name,
                Address = parkingZone.Address,
                DateOfEstablishment = parkingZone.DateOfEstablishment
            });
            return listItemVMs;
        }
    }
}