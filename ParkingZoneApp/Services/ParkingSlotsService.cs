using ParkingZoneApp.Models;
using ParkingZoneApp.Repositories;

namespace ParkingZoneApp.Services
{
    public class ParkingSlotsService : Service<ParkingSlots>, IParkingSlotsService
    {
        public ParkingSlotsService(IParkingSlotsRepository repository)
        :base(repository){ }
    }
}
