using ParkingZoneApp.Data;
using ParkingZoneApp.Models;

namespace ParkingZoneApp.Repositories
{
    public interface IParkingZoneRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int? id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int? id);
    }
}
