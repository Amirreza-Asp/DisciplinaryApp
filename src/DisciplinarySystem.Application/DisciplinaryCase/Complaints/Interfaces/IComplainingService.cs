using DisciplinarySystem.Domain.Complaints;

namespace DisciplinarySystem.Application.Complaints.Interfaces
{
    public interface IComplainingService
    {
        Task<Complaining> GetByCaseIdAsync(long caseId);
    }
}
