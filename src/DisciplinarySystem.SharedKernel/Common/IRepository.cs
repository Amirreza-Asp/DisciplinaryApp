using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DisciplinarySystem.SharedKernel.Common
{
    public interface IRepository<T> where T : class
    {
        T Find(object id);

        Task<T> FindAsync(object id);

        int GetCount(Expression<Func<T, bool>> filter = null);

        IEnumerable<U> GetAll<U>
            (
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                Expression<Func<T, U>> select = null,
                bool isTracking = false,
                int take = 0,
                int skip = 0
            );

        Task<IEnumerable<U>> GetAllAsync<U>
            (
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                Expression<Func<T, U>> select = null,
                bool isTracking = false,
                int take = 0,
                int skip = 0
            );

        IEnumerable<T> GetAll
            (
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                bool isTracking = false,
                int take = 0,
                int skip = 0
            );

        Task<IEnumerable<T>> GetAllAsync
            (
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                bool isTracking = false,
                int take = 0,
                int skip = 0
            );

        T FirstOrDefault
            (
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                bool isTracking = false
            );

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                bool isTracking = false);

        Task<U> FirstOrDefaultSelectAsync<U>(
            Expression<Func<T, U>> select,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>,
            IIncludableQueryable<T, object>> include = null,
            bool isTracking = false);

        U FirstOrDefaultSelect<U>(
            Expression<Func<T, U>> select,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>,
            IIncludableQueryable<T, object>> include = null,
            bool isTracking = false);

        void Remove(T entity);

        void Add(T entity);

        void Update(T entity);

        void Save();

        Task SaveAsync();
    }
}
