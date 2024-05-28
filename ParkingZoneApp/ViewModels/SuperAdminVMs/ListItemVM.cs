using ParkingZoneApp.Models;

namespace ParkingZoneApp.ViewModels.SuperAdminVMs
{
    public class ListItemVM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public IList<string> Role { get; set; }

        public ListItemVM(ApplicationUser user)
        {
            Id = user.Id;
            Name = user.Name;
            UserName = user.UserName;
            PhoneNumber = user.PhoneNumber;
            Role = new List<string>();
        }

        public ListItemVM() { }
    }
}
