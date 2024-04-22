using ParkingZoneApp.Models;
using ParkingZoneApp.Repositories;

namespace ParkingZoneApp.Services
{
    public interface IParkingSlotService : IService<ParkingSlot>
    {
        public IEnumerable<ParkingSlot> GetByParkingZoneId(int parkingZoneId);
    }
}
