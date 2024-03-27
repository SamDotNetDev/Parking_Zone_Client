
using Microsoft.EntityFrameworkCore;
using ParkingZoneApp.Data;
using System;

namespace ParkingZoneApp.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            this._context = context;
            this._dbSet = _context.Set<T>();
        }
        public void Delete(T Entity)
        {
            _dbSet.Remove(Entity);
            _context.SaveChanges();
        }
        public IEnumerable<T> GetAll()
        {
            return _dbSet;
        }

        public T GetById(int? id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(T entity)
        {
            _dbSet.AddAsync(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
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
