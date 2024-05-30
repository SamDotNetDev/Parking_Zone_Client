using ParkingZoneApp.Enums;
using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.SuperAdminVMs
{
    public class PromoteVM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public RolesEnum Role { get; set; }

        public PromoteVM(ApplicationUser user)
        {
            Id = user.Id;
            Name = user.Name;
        }

        public PromoteVM() { }
    }
}
