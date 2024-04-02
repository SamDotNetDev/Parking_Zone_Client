using ParkingZoneApp.Models;
using ParkingZoneApp.Repositories;

namespace ParkingZoneApp.Services
{
    public class ParkingZoneService : Service<ParkingZone>, IParkingZoneService
    {
        public ParkingZoneService(IParkingZoneRepository repository)
            : base(repository) 
        {
        
        }

        public override void Insert(ParkingZone entity)
        {
            entity.DateOfEstablishment = DateTime.Now;
            base.Insert(entity);
        }
    }
}
