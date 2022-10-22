using DisciplinarySystem.Application.Complaints.ViewModels;
using DisciplinarySystem.Application.Complaints.ViewModels.Create;
using DisciplinarySystem.Application.Complaints.ViewModels.Update;
using DisciplinarySystem.Domain.Complaints;
using DisciplinarySystem.SharedKernel.Common;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Complaints.Interfaces
{
    public interface IComplaintService
    {
        Task<UserInfo> GetUserAsync(String nationalCode);
        Task<Complaint> GetByIdAsync(long id);
        Task<List<GetComplaint>> GetAllAsync(Expression<Func<Complaint, bool>> filter = null, int take = 10, int skip = 0);
        Task<Complaint> GetByCaseIdAsync(long caseId);
        int GetCount(Expression<Func<Complaint, bool>> filter = null);
        Task<ComplaintDocument> GetDocumentByIdAsync(Guid id);


        Task CreateAsync(CreateComplaint createComplaint, IFormFileCollection files);
        Task UpdateAsync(UpdateComplaint updateComplaint, IFormFileCollection files);
        Task<bool> RemoveAsync(long id);
        Task<bool> RemoveFileAsync(Guid guid);
        Task CreateCaseAsync(long id);
    }
}
