namespace DisciplinarySystem.Domain.Epistles.Interfaces
{
    public interface IEpistleRepository : IRepository<Epistle>
    {
        Task<long> CreateAsync(Epistle entity);
    }
}
