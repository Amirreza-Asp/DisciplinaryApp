using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Common.Interfaces
{
    public interface IServiceBase<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<List<T>> GetListAsync(int skip = 0, int take = 10);
        int GetCount(Expression<Func<T, bool>> filter = null);

        Task<bool> RemoveAsync(Guid id);
    }
}
