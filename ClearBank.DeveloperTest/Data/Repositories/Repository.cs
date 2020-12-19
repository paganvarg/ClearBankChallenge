using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ClearBank.DeveloperTest.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ClearBankContext _context;

        public Repository(ClearBankContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Add)} {nameof(entity)} must not be null");
            }

            var item = _context.Add(entity);
            _context.SaveChanges();
            return item.Entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} {nameof(entity)} must not be null");
            }

            var item = await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return item.Entity;
        }

        public TEntity Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Update)} {nameof(entity)} must not be null");
            }

            var item = _context.Update(entity);
            _context.SaveChanges();
            return item.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(UpdateAsync)} {nameof(entity)} must not be null");
            }

            var item = _context.Update(entity);
            await _context.SaveChangesAsync();
            return item.Entity;
        }
    }
}
