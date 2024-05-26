using ParkingZoneApp.Enums;
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

        public ZoneFinanceData GetZoneFinanceDataByPeriod(PeriodEnum period, ParkingZone zone)
        {
            var zoneFinanceData = new ZoneFinanceData();
            var slots = zone.ParkingSlots;
            var Now = DateTime.Now;

            DateTime periodStart, periodEnd;
            (periodStart, periodEnd) = period switch
            {
                PeriodEnum.Today => (Now.Date, Now.Date.AddDays(1)),
                PeriodEnum.Yesterday => (Now.Date.AddDays(-1), Now.Date),
                PeriodEnum.Last7Days => (Now.Date.AddDays(-7), Now.Date.AddDays(1)),
                PeriodEnum.Last30Days => (Now.Date.AddDays(-30), Now.Date.AddDays(1)),
                _ => (DateTime.MinValue, DateTime.MaxValue)
            };

            zoneFinanceData.CategoryHours = slots
                .GroupBy(s => s.Category)
                .ToDictionary(
                    g => g.Key,
                    g => g.SelectMany(slot => slot.Reservations
                        .Where(r => r.StartTime >= periodStart && r.StartTime < periodEnd))
                        .Sum(r => r.Duration));

            foreach (SlotCategoryEnum category in Enum.GetValues(typeof(SlotCategoryEnum)))
            {
                if (!zoneFinanceData.CategoryHours.ContainsKey(category))
                {
                    zoneFinanceData.CategoryHours[category] = 0;
                }
            }

            return zoneFinanceData;
        }
    }
}
