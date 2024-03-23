using ParkingZoneApp.Data;
using ParkingZoneApp.Models;

namespace ParkingZoneApp.Repositories
{
    public interface IGenericParkingZoneRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Insert(T parkingZone);
        void Update(T parkingZone);
        void Delete(int id);
        void Save();
    }
}
