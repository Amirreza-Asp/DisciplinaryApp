namespace DisciplinarySystem.Domain.Complaints.Interfaces
{
    public interface IComplaintRepository : IRepository<Complaint>
    {
        Task<long> CreateAsync(Complaint complaint);
        bool IsExists(long id);
    }
}
