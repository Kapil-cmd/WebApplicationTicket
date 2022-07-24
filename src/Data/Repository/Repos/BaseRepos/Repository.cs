
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository.Repos.Reposi
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TicketingContext _db;
        private DbSet<T> dbSet;

        public Repository(TicketingContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public bool Any(Expression<Func<T, bool>> filter)
        {
            return dbSet.Any(filter);
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }
        public T GetFirstOrDefault(Expression<Func<T,bool>>filter)
        {
            IQueryable<T> query = dbSet;

            query = query.Where(filter);
            return query.FirstOrDefault();
        }
        public void Update(T entity)
        {
            dbSet.Update(entity);
        }
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
        public async Task<T> GetById<TId>(TId id)
        {
            return await _db.Set<T>().FindAsync(id);
        }
    }
}
