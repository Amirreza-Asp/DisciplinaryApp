using DisciplinarySystem.Domain.Dtos;

namespace DisciplinarySystem.Application.Contracts.Interfaces
{
    public interface IPositionAPI
    {
        Task<IEnumerable<UserByPosition>> GetUsersAsync ( String position );

        Task<IEnumerable<Position>> GetPositionsAsync ();
    }
}
