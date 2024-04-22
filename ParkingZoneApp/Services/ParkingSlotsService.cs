using ParkingZoneApp.Models;
using ParkingZoneApp.Repositories;

namespace ParkingZoneApp.Services
{
    public class ParkingSlotsService : Service<ParkingSlots>, IParkingSlotsService
    {
        public ParkingSlotsService(IParkingSlotsRepository repository)
        :base(repository){ }

        public IEnumerable<ParkingSlots> GetByParkingZoneId(int parkingZoneId)
        {
            var ParkingSlots = _repository
                .GetAll().Where(x=>x.ParkingZoneId == parkingZoneId);
            return ParkingSlots;
        }
    }
}
