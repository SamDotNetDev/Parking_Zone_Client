using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingZonesVMs
{
    public class ListItemVM
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public ICollection<ParkingSlot> ParkingSlots { get; set; }

        public int FreeSlots
        {
            get => FreeSlot();
        }

        public int SlotsInUse
        {
            get => SlotInUse();
        }

        public ListItemVM() { }

        public ListItemVM(ParkingZone parkingZone)
        {
            Id = parkingZone.Id;
            Name = parkingZone.Name;
            Address = parkingZone.Address;
            ParkingSlots = parkingZone.ParkingSlots;
        }
        private int SlotInUse()
        {
            int Number = 0;
            foreach (var slot in ParkingSlots)
            {
                if (slot.IsInUse) Number++;
            }
            return Number;
        }
        private int FreeSlot()
        {
            int Number = 0;
            foreach (var slot in ParkingSlots)
            {
                if (!slot.IsInUse) Number++;
            }
            return Number;
        }
    }
}