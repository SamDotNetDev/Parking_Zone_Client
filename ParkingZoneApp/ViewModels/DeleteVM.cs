
namespace ParkingZoneApp.ViewModels
{
    public class DeleteVM
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateOfEstablishment { get; set; } = DateTime.Now;
    }
}
