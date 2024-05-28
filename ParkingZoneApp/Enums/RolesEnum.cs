namespace ParkingZoneApp.Enums
{
    public enum RolesEnum
    {
        User = 1,
        Admin,
        SuperAdmin
    }

    public class RoleConverter
    {
        public RolesEnum ConvertToUserRole(string roleName)
        {
            switch (roleName)
            {
                case "SuperAdmin":
                    return RolesEnum.SuperAdmin;
                case "Admin":
                    return RolesEnum.Admin;
                case "User":
                    return RolesEnum.User;
                default:
                    throw new ArgumentException($"Unknown role: {roleName}");
            }
        }
    }
}
