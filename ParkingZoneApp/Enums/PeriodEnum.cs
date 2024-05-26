using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.Enums
{
    public enum PeriodEnum
    {
        [Display(Name = "All Time")]
        AllTime,

        Today,
        Yesterday,

        [Display(Name = "Last 7 Days")]
        Last7Days,

        [Display(Name = "Last 30 Days")]
        Last30Days
    }
}
