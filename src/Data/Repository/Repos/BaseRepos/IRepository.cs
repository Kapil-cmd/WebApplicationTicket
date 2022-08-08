

using System.Linq.Expressions;

namespace Repository.Repos.BaseRepos
{
    public interface IRepository <T> where T : class
    {
        T GetFirstOrDefault(Expression<Func<T, bool>> filter);
        Task<T> GetById<TId>(TId id);
        IEnumerable<T> GetAll();
        bool Any(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
