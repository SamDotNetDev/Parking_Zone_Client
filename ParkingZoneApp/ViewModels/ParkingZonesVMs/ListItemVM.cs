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
            get => CountFreeSlot();
        }

        public int SlotsInUse
        {
            get => CountSlotInUse();
        }

        public ListItemVM() { }

        public ListItemVM(ParkingZone parkingZone)
        {
            Id = parkingZone.Id;
            Name = parkingZone.Name;
            Address = parkingZone.Address;
            ParkingSlots = parkingZone.ParkingSlots;
        }
        private int CountSlotInUse()
        {
            int Number = 0;
            foreach (var slot in ParkingSlots)
            {
                if (slot.IsInUse) Number++;
            }
            return Number;
        }
        private int CountFreeSlot()
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