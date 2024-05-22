using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;

namespace ParkingZoneApp.Services
{
    public interface IParkingSlotService : IService<ParkingSlot>
    {
        public IEnumerable<ParkingSlot> GetByParkingZoneId(int parkingZoneId);
        public bool ParkingSlotExists(int parkingZoneId, int parkingSlotNumber);
        public bool IsSlotFreeForReservation(ParkingSlot slot, DateTime starTime, int duration);
        public IEnumerable<ParkingSlot> GetFreeByParkingZoneIdAndPeriod(int parkingZoneId, DateTime startTime, int duration);
        public IQueryable<ParkingSlot> FilterByCategory(IQueryable<ParkingSlot> query, SlotCategoryEnum? category);
        public IQueryable<ParkingSlot> FilterByFreeSlot(IQueryable<ParkingSlot> query, bool? IsSlotFree);
    }
}