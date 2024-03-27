using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using ParkingZoneApp.Data;
using ParkingZoneApp.Models;
using System.Data;

namespace ParkingZoneApp.Repositories
{
    public class ParkingZoneRepository : Repository<ParkingZone>,IParkingZoneRepository
    {
        public ParkingZoneRepository(ApplicationDbContext options)
            : base(options) { }
    }
}
