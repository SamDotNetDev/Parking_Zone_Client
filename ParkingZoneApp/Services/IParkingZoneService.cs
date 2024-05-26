using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;

namespace ParkingZoneApp.Services
{
    public interface IParkingZoneService : IService<ParkingZone>
    {
        public ZoneFinanceData GetZoneFinanceDataByPeriod(PeriodEnum period, ParkingZone zone);
    }
}
