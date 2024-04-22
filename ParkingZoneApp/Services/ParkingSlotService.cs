using ParkingZoneApp.Models;
using ParkingZoneApp.Repositories;

namespace ParkingZoneApp.Services
{
    public class ParkingSlotService : Service<ParkingSlot>, IParkingSlotService
    {
        public ParkingSlotService(IParkingSlotsRepository repository)
        :base(repository){ }

        public IEnumerable<ParkingSlot> GetByParkingZoneId(int parkingZoneId)
        {
            var ParkingSlots = _repository
                .GetAll().Where(x => x.ParkingZoneId == parkingZoneId);
            return ParkingSlots;
        }
    }
}
