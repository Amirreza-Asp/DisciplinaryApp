using DisciplinarySystem.Application.Common.Interfaces;
using DisciplinarySystem.SharedKernel.Common;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Common
{
    public class ServiceBase<T> : IServiceBase<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public ServiceBase(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _repository.FindAsync(id);
        }

        public int GetCount(Expression<Func<T, bool>> filter = null)
        {
            return _repository.GetCount(filter);
        }

        public async Task<List<T>> GetListAsync(int skip = 0, int take = 10)
        {
            var objs = await _repository.GetAllAsync(skip: skip, take: take);
            return objs.ToList();
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            T entity = await GetByIdAsync(id);

            if (entity == null)
                return false;

            _repository.Remove(entity);
            await _repository.SaveAsync();
            return true;
        }
    }
}
