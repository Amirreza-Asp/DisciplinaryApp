using DisciplinarySystem.Application.Meetings.ViewModels;
using DisciplinarySystem.Domain.Meetings;
using DisciplinarySystem.SharedKernel.Common;

namespace DisciplinarySystem.Application.Meetings.Interfaces
{
	public class ProceedingsService : IProceedingsService
	{
		private readonly IRepository<Proceedings> _prcRepo;

		public ProceedingsService(IRepository<Proceedings> prcRepo)
		{
			_prcRepo = prcRepo;
		}

		public async Task<Proceedings> GetByMeetingIdAsync(Guid id)
		{
			return await _prcRepo.FirstOrDefaultAsync(u => u.MeetingId == id);
		}
		public async Task<Proceedings> GetByIdAsync(Guid id)
		{
			return await _prcRepo.FirstOrDefaultAsync(u => u.Id == id);
		}

		public async Task CreateAsync(CreateProceedings command)
		{
			var entity = new Proceedings(command.Title, command.Description, command.MeetingId);
			_prcRepo.Add(entity);
			await _prcRepo.SaveAsync();
		}
		public async Task UpdateAsync(UpdateProccedings command)
		{
			var entity = await GetByIdAsync(command.Id);
			if (entity == null)
				return;

			entity.WithTitle(command.Title)
				.WithDescription(command.Description);

			_prcRepo.Update(entity);
			await _prcRepo.SaveAsync();
		}

		public async Task<bool> RemoveByMeetingIdAsync(Guid meetingId)
		{
			var entity = await GetByMeetingIdAsync(meetingId);
			if (entity == null)
				return false;

			_prcRepo.Remove(entity);
			await _prcRepo.SaveAsync();
			return true;
		}
	}
}
