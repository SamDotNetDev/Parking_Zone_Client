using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Models;

namespace ParkingZoneApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        public DbSet<ParkingZone> ParkingZone { get; set; }
        public DbSet<ParkingSlot> ParkingSlots { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}