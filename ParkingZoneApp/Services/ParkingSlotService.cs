using ParkingZoneApp.Data.Migrations;
using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;
using ParkingZoneApp.Repositories;

namespace ParkingZoneApp.Services
{
    public class ParkingSlotService : Service<ParkingSlot>, IParkingSlotService
    {
        public ParkingSlotService(IParkingSlotsRepository repository)
        :base(repository){ }

        public IEnumerable<ParkingSlot> GetByParkingZoneId(int parkingZoneId)
        {
            var ParkingSlots = _repository
                .GetAll().Where(x => x.ParkingZoneId == parkingZoneId);
            return ParkingSlots;
        }

        public bool ParkingSlotExists(int parkingZoneId, int parkingSlotNumber)
        {
            var ParkingSlots = _repository.GetAll()
                .Where(x => x.ParkingZoneId == parkingZoneId &&
                x.Number == parkingSlotNumber);
            return ParkingSlots.Count() == 0 ? false : true;
        }

        public IEnumerable<ParkingSlot> GetFreeByParkingZoneIdAndPeriod(int parkingZoneId, DateTime startTime, int duration)
        {
            var slots = _repository.GetAll()
                .Where(x => x.ParkingZoneId == parkingZoneId 
                && x.IsAvailableForBooking
                && IsSlotFreeForReservation(x, startTime, duration));
            return slots;
        }

        public bool IsSlotFreeForReservation(ParkingSlot slot, DateTime startTime, int duration)
        {
            DateTime endTime = startTime.AddHours(duration);

            return !slot.Reservations.Any(r =>
                (startTime >= r.StartTime && startTime < r.StartTime.AddHours(r.Duration)) ||
                (endTime > r.StartTime && endTime <= r.StartTime.AddHours(r.Duration)) ||
                (startTime <= r.StartTime && endTime >= r.StartTime.AddHours(r.Duration)));
        }

        public IQueryable<ParkingSlot> FilterByCategory(IQueryable<ParkingSlot> query, SlotCategoryEnum? category)
        {
            if (category.HasValue)
            {
                query = query.Where(x => x.Category == category.Value);
            }
            return query;
        }

        public IQueryable<ParkingSlot> FilterByFreeSlot(IQueryable<ParkingSlot> query, bool? isSlotFree)
        {
            if (isSlotFree.HasValue)
            {
                query = query.Where(x => !x.IsInUse == isSlotFree.Value);
            }
            return query;
        }
    }
}
