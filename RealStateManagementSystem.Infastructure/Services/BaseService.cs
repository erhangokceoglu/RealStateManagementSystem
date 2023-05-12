using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealStateManagementSystem.Domain.Entities;
using RealStateManagementSystem.Domain.Interfaces;
using RealStateManagementSystem.Domain.IServices;
using RealStateManagementSystem.Infastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagementSystem.Infastructure.Services
{
    public class BaseService<T, TContext> : IBaseService<T> where T : class where TContext : ApplicationDbContext
    {
        private readonly ApplicationDbContext _db;

        public BaseService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> AddAsync(T entity)
        {
            _db.Set<T>().Add(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var getId = await _db.Set<T>().FindAsync(id);
            _db.Set<T>().Remove(getId!);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
        {
            return await _db.Set<T>().AnyAsync(filter);
        }

        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            if (filter != null)
                return await _db.Set<T>().Where(filter).ToListAsync();
            else
                return await _db.Set<T>().ToListAsync();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _db.Set<T>().Update(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _db.Set<T>().FindAsync(id);
            return entity!;
        }
    }
}
