using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using ParkingZoneApp.Data;
using ParkingZoneApp.Models;
using System.Data;

namespace ParkingZoneApp.Repositories
{
    public class ParkingZoneRepository : IGenericParkingZoneRepository<ParkingZone>
    {
        private readonly ApplicationDbContext context;
        public ParkingZoneRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        void IGenericParkingZoneRepository<ParkingZone>.Delete(int id)
        {
            var ParkingZoneInDb = context.ParkingZone.Find(id);
            if (ParkingZoneInDb != null)
            {
                context.Remove(ParkingZoneInDb);
            }
        }

        IEnumerable<ParkingZone> IGenericParkingZoneRepository<ParkingZone>.GetAll()
        {
            return context.ParkingZone;
        }

        ParkingZone IGenericParkingZoneRepository<ParkingZone>.GetById(int id)
        {
            return context.ParkingZone.Find(id);
        }

        void IGenericParkingZoneRepository<ParkingZone>.Insert(ParkingZone parkingZone)
        {
            parkingZone.DateOfEstablishment = DateTime.Now;
            context.Add(parkingZone);
        }

        void IGenericParkingZoneRepository<ParkingZone>.Save()
        {
            context.SaveChangesAsync();
        }

        void IGenericParkingZoneRepository<ParkingZone>.Update(ParkingZone parkingZone)
        {
            if (parkingZone.DateOfEstablishment > DateTime.Now)
            {
                parkingZone.DateOfEstablishment = DateTime.Now;
            }
            var res = context.ParkingZone.Find(parkingZone.Id);
            if (res != null)
            {
                res.Name = parkingZone.Name;
                res.Address = parkingZone.Address;
                res.DateOfEstablishment = parkingZone.DateOfEstablishment;
                context.Update(res);
            }
        }
    }
}
