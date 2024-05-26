using ParkingZoneApp.Enums;

namespace ParkingZoneApp.Models
{
    public class ZoneFinanceData
    {
        public Dictionary<SlotCategoryEnum, int> CategoryHours { get; set; }
    }
}
