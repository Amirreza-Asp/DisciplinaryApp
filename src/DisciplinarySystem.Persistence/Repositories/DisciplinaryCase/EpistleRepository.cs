using DisciplinarySystem.Domain.Epistles;
using DisciplinarySystem.Domain.Epistles.Interfaces;
using DisciplinarySystem.Persistence.Data;

namespace DisciplinarySystem.Persistence.Repositories.DisciplinaryCase
{
    public class EpistleRepository : Repository<Epistle>, IEpistleRepository
    {
        private readonly ApplicationDbContext _context;
        public EpistleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<long> CreateAsync(Epistle entity)
        {
            var entry = _context.Add(entity);
            await _context.SaveChangesAsync();
            return entry.Entity.Id;
        }

        public override void Update(Epistle entity)
        {
            entity.WithUpdateDate(DateTime.Now);
            base.Update(entity);
        }
    }
}
