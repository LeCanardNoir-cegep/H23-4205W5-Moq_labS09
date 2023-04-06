using Microsoft.EntityFrameworkCore;
using PresseMots_DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresseMots_DataAccess.Services
{

    public interface IServiceBaseAsync<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task DeleteAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task EditAsync(T entity);
    }

    public interface IServiceBase<T> where T : class
    {
        T Create(T entity);
        void Delete(int id);
        IReadOnlyList<T> GetAll();
        T GetById(int id);
        void Edit(T entity);
    }



    public class ServiceBaseEF<T> : IServiceBaseAsync<T>, IServiceBase<T> where T : class
    {

        protected readonly PresseMotsDbContext _dbContext;

        public ServiceBaseEF(PresseMotsDbContext dbContext) => _dbContext = dbContext;

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public virtual T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }


        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public virtual IReadOnlyList<T> GetAll()
        {
            return _dbContext.Set<T>().ToList();
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
        public virtual T Create(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }
        public virtual async Task EditAsync(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached) _dbContext.Update<T>(entity);
            else _dbContext.Entry(entity).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }


        public virtual void Edit(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached) _dbContext.Update<T>(entity);
            else _dbContext.Entry(entity).State = EntityState.Modified;


            _dbContext.SaveChanges();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await this.GetByIdAsync(id);
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual void Delete(int id)
        {
            var entity = this.GetById(id);
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

    }
}
