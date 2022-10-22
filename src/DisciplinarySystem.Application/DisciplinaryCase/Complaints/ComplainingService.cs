using DisciplinarySystem.Application.Complaints.Interfaces;
using DisciplinarySystem.Domain.Complaints;
using DisciplinarySystem.SharedKernel.Common;

namespace DisciplinarySystem.Application.Complaints
{
    public class ComplainingService : IComplainingService
    {
        private readonly IRepository<Complaining> _cmpRepo;

        public ComplainingService(IRepository<Complaining> cmpRepo)
        {
            _cmpRepo = cmpRepo;
        }

        public async Task<Complaining> GetByCaseIdAsync(long caseId)
        {
            return await _cmpRepo.FirstOrDefaultAsync(u => u.Complaint.Case.Id == caseId);
        }
    }
}
