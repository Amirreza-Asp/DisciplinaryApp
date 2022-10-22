using DisciplinarySystem.Application.Meetings.ViewModels;
using DisciplinarySystem.Domain.Meetings;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Meetings.Interfaces
{
    public interface IMeetingService
    {
        Task<IEnumerable<Meeting>> ListAsync(Expression<Func<Meeting, bool>> filters = null);
        Task<Meeting> GetByIdAsync(Guid id);
        int GetCount(Expression<Func<Meeting, bool>> filters = null);

        Task CreateAsync(CreateMeeting command);
        Task UpdateAsync(UpdateMeeting command);
        Task<bool> RemoveAsync(Guid id);
        Task<bool> RemoveUserAsync(Guid userId);
    }
}
