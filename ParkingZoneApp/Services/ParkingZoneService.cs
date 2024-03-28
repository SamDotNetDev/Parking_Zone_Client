using ParkingZoneApp.Models;
using ParkingZoneApp.Repositories;

namespace ParkingZoneApp.Services
{
    public class ParkingZoneService : IParkingZoneService
    {
        private readonly IRepository<ParkingZone>  _parkingZoneRepository;

        public ParkingZoneService(IRepository<ParkingZone> parkingZoneRepository)
        {
            _parkingZoneRepository = parkingZoneRepository;
        }

        public void Delete(int Id)
        {
            var ParkingZone = _parkingZoneRepository.GetById(Id);
            _parkingZoneRepository.Delete(ParkingZone);
        }

        public IEnumerable<ParkingZone> GetAll()
        {
            return _parkingZoneRepository.GetAll();
        }

        public ParkingZone GetById(int? id)
        {
            return _parkingZoneRepository.GetById(id);
        }

        public void Insert(ParkingZone zone)
        {
            _parkingZoneRepository.Insert(zone);
        }

        public void Update(int id, ParkingZone zone)
        {
            if(id==zone.Id)
            {
                _parkingZoneRepository.Update(zone);
            }
        }
    }
}
