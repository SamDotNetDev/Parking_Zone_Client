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

        public bool ParkingSlotExits(int ParkingZoneId, int ParkingSlotNumber)
        {
            var ParkingSlots = _repository.GetAll()
                .Where(x => x.ParkingZoneId == ParkingZoneId &&
                x.Number == ParkingSlotNumber);
            return ParkingSlots.Count() == 0 ? false : true;
        }
    }
}
