using DisciplinarySystem.Application.Invitations.ViewModels;
using DisciplinarySystem.Domain.Invitations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Invitations.Interfaces
{
    public interface IInvitationService
    {
        Task<IEnumerable<InvitationDetails>> ListAsync(Expression<Func<Invitation, bool>> filters = null, int skip = 0, int take = 10);
        Task<List<SelectListItem>> GetPersonsAsync(long caseId);
        Task<List<InvitationDocument>> GetDocumentsAsync(Guid id);
        Task<Invitation> GetByIdAsync(Guid id);
        Task<List<InvitationsInfo>> GetInvitePersonsAsync(Guid id);
        int GetCount(Expression<Func<Invitation, bool>> filter = null);
       Task<InvitationDocument> GetDocumentByIdAsync(Guid id);

        Task CreateAsync(CreateInvitation command, IFormFileCollection files);
        Task UpdateAsync(UpdateInvitation command, IFormFileCollection files);
        Task<bool> RemoveAsync(Guid id);
        Task RemovePersonAsync(Guid invId, Guid personId, string group);
        Task<bool> RemoveFileAsync(Guid id);
    }
}
