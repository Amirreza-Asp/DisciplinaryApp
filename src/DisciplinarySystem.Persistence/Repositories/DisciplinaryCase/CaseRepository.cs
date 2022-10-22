using DisciplinarySystem.Domain.DisciplinaryCase.Cases;
using DisciplinarySystem.Domain.DisciplinaryCase.Cases.Enums;
using DisciplinarySystem.Domain.DisciplinaryCase.Cases.Interfaces;
using DisciplinarySystem.Persistence.Data;
using System.Linq.Expressions;

namespace DisciplinarySystem.Persistence.Repositories.DisciplinaryCase
{
    public class CaseRepository : Repository<Case>, ICaseReposiotry
    {
        private readonly ApplicationDbContext _context;
        public CaseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public bool IsExist(long id)
        {
            return _context.Cases.Any(c => c.Id == id);
        }

        public bool IsExist(Expression<Func<Case, bool>> filters)
        {
            return _context.Cases.Where(filters).Any();
        }

        public void UpdateStatus(long caseId, CaseStatus status)
        {
            var entity = _context.Cases.FirstOrDefault(u => u.Id == caseId);
            if (entity == null)
                return;

            entity.WithStatus(status);
        }
    }
}
