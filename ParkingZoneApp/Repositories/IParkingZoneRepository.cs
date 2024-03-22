using ParkingZoneApp.Data;
using ParkingZoneApp.Models;

namespace ParkingZoneApp.Repositories
{
    public interface IParkingZoneRepository
    {
        IEnumerable<ParkingZone> GetAll();
        ParkingZone GetById(int id);

        void Insert(ParkingZone parkingZone);

        void Update(ParkingZone parkingZone);
        void Delete(int id);
        void Save();
    }
}
