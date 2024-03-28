using ParkingZoneApp.Models;

namespace ParkingZoneApp.Services
{
    public interface IParkingZoneService
    {
        IEnumerable<ParkingZone> GetAll();
        ParkingZone GetById(int? id);
        void Insert(ParkingZone zone);
        void Update(int id,ParkingZone zone);
        void Delete(int id);
    }
}
