using ParkingZoneApp.Models;
using ParkingZoneApp.Repositories;

namespace ParkingZoneApp.Services
{
    public interface IParkingSlotsService : IService<ParkingSlots>
    {
        public IEnumerable<ParkingSlots> GetByParkingZoneId(int parkingZoneId);
    }
}
