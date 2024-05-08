using ParkingZoneApp.Data;
using ParkingZoneApp.Models;

namespace ParkingZoneApp.Repositories
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(ApplicationDbContext context) : base(context) { }
    }
}
