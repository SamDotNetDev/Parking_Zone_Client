using ParkingZoneApp.Models;
using ParkingZoneApp.Repositories;

namespace ParkingZoneApp.Services
{
    public class ReservationService : Service<Reservation>, IReservationService
    {
        public ReservationService(IReservationRepository repository)
        : base(repository) { }

        public IEnumerable<Reservation> ReservationsByUserId(string UserId)
        {
            var UserReservations = GetAll().Where(x => x.UserId == UserId);
            return UserReservations;
        }

        public bool IsDateInvalid(DateTime date)
        {
            string NowDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            return date < DateTime.Parse(NowDate);
        }
    }
}
