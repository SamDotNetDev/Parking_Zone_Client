
namespace ParkingZoneApp.ViewModels
{
    public class DetailsVM
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateOfEstablishment { get; set; } = DateTime.Now;
    }
}
