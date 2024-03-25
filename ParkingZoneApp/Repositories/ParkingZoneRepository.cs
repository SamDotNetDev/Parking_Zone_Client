using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using ParkingZoneApp.Data;
using ParkingZoneApp.Models;
using System.Data;

namespace ParkingZoneApp.Repositories
{
    public class GenericParkingZoneRepository : IGenericParkingZoneRepository<ParkingZone>
    {
        private readonly ApplicationDbContext context;
        public GenericParkingZoneRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        void IGenericParkingZoneRepository<ParkingZone>.Delete(int? id)
        {
            var ParkingZoneInDb = context.ParkingZone.Find(id);
            if (ParkingZoneInDb != null)
            {
                context.Remove(ParkingZoneInDb);
                context.SaveChanges();
            }
        }

        IEnumerable<ParkingZone> IGenericParkingZoneRepository<ParkingZone>.GetAll()
        {
            return context.ParkingZone;
        }

        ParkingZone IGenericParkingZoneRepository<ParkingZone>.GetById(int? id)
        {

            return context.ParkingZone.Find(id);
        }

        void IGenericParkingZoneRepository<ParkingZone>.Insert(ParkingZone parkingZone)
        {
            parkingZone.DateOfEstablishment = DateTime.Now;
            context.AddAsync(parkingZone);
            context.SaveChanges();
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
                context.SaveChanges();
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
