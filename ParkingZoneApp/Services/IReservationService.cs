using ParkingZoneApp.Models;

namespace ParkingZoneApp.Services
{
    public interface IReservationService : IService<Reservation>
    {
        public IEnumerable<Reservation> ReservationsByUserId(string userId);

        public bool IsDateInvalid(DateTime date);
    }
}
