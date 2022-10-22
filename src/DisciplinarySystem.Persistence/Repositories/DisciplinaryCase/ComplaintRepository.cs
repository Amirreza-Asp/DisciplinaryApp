using DisciplinarySystem.Domain.Complaints;
using DisciplinarySystem.Domain.Complaints.Interfaces;
using DisciplinarySystem.Persistence.Data;

namespace DisciplinarySystem.Persistence.Repositories.DisciplinaryCase
{
    public class ComplaintRepository : Repository<Complaint>, IComplaintRepository
    {
        private readonly ApplicationDbContext _context;
        public ComplaintRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<long> CreateAsync(Complaint complaint)
        {
            var entry = _context.Add(complaint);
            await _context.SaveChangesAsync();
            return entry.Entity.Id;
        }

        public bool IsExists(long id)
        {
            return _context.Complaints.Any(u => u.Id == id);
        }
    }
}
