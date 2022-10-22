using DisciplinarySystem.Application.Epistles.ViewModels;
using DisciplinarySystem.Domain.Epistles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Epistles.Interfaces
{
	public interface IEpistleService
	{
		Task<IEnumerable<GetEpistle>> GetAllAsync(Expression<Func<Epistle, bool>> filter = null, int skip = 0, int take = 10);
		Task<Epistle> GetByIdAsync(long id);
		Task<List<GetInformedDocument>> GetCurrentDocumentsAsync(long epistleId);
		int GetCount(Expression<Func<Epistle, bool>> filter = null);
        Task<EpistleDocument> GetDocumentByIdAsync(Guid id);

        bool HasComplaint(long id);
		bool HasCase(long id);

		Task CreateAsync(CreateEpistle command, IFormFileCollection files);
		Task UpdateAsync(UpdateEpistle command, IFormFileCollection files);
		Task<bool> RemoveAsync(long id);
		Task<bool> RemoveFileAsync(Guid id);
    }
}
