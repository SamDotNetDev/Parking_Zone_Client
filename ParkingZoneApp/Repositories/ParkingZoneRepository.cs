using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using ParkingZoneApp.Data;
using ParkingZoneApp.Models;
using System.Data;

namespace ParkingZoneApp.Repositories
{
    public class ParkingZoneRepository : IParkingZoneRepository
    {
        private readonly ApplicationDbContext context;
        public ParkingZoneRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        void IParkingZoneRepository.Delete(int id)
        {
            var ParkingZoneInDb = context.ParkingZone.Find(id);
            if (ParkingZoneInDb != null)
            {
                context.Remove(ParkingZoneInDb);
            }
        }

        IEnumerable<ParkingZone> IParkingZoneRepository.GetAll()
        {
            return context.ParkingZone;
        }

        ParkingZone IParkingZoneRepository.GetById(int id)
        {
            return context.ParkingZone.Find(id);
        }

        void IParkingZoneRepository.Insert(ParkingZone parkingZone)
        {
            parkingZone.DateOfEstablishment = DateTime.Now;
            context.Add(parkingZone);
        }

        void IParkingZoneRepository.Save()
        {
            context.SaveChangesAsync();
        }

        void IParkingZoneRepository.Update(ParkingZone parkingZone)
        {
            if (parkingZone.DateOfEstablishment > DateTime.Now)
            {
                parkingZone.DateOfEstablishment = DateTime.Now;
            }
            context.Entry(parkingZone).State = EntityState.Modified;
        }
    }
}
