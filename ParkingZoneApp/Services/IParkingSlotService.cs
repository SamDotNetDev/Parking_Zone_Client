using ParkingZoneApp.Models;

namespace ParkingZoneApp.Services
{
    public interface IParkingSlotService : IService<ParkingSlot>
    {
        public IEnumerable<ParkingSlot> GetByParkingZoneId(int parkingZoneId);
        public bool ParkingSlotExits(int parkingZoneId, int parkingSlotNumber);
        public bool SlotFree(ParkingSlot slot, DateTime starTime, int duration);
        public IEnumerable<ParkingSlot> GetFreeByParkingZoneIdAndPeriod(int parkingZoneId, DateTime startTime, int duration);
    }
}