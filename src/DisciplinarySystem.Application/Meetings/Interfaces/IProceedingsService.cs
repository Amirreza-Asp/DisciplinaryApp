using DisciplinarySystem.Application.Meetings.ViewModels;
using DisciplinarySystem.Domain.Meetings;

namespace DisciplinarySystem.Application.Meetings.Interfaces
{
	public interface IProceedingsService
	{
		Task<Proceedings> GetByMeetingIdAsync(Guid id);
		Task<Proceedings> GetByIdAsync(Guid id);


		Task CreateAsync(CreateProceedings command);
		Task UpdateAsync(UpdateProccedings command);
		Task<bool> RemoveByMeetingIdAsync(Guid meetingId);
	}
}
