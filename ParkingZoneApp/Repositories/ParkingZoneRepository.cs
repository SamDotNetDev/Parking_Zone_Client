using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using ParkingZoneApp.Data;
using ParkingZoneApp.Models;
using System.Data;

namespace ParkingZoneApp.Repositories
{
    public class GenericParkingZoneRepository : Repository<ParkingZone>,IParkingZoneRepository
    {
        public GenericParkingZoneRepository(ApplicationDbContext options)
            : base(options) { }
    }
}
