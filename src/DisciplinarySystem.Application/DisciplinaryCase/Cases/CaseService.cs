using DisciplinarySystem.Application.Cases.Helpers;
using DisciplinarySystem.Application.Cases.Interfaces;
using DisciplinarySystem.Application.Cases.ViewModels;
using DisciplinarySystem.Domain.Complaints.Enums;
using DisciplinarySystem.Domain.Complaints.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Cases
{
    public class CaseService : ICaseService
    {
        private readonly ICaseReposiotry _caseRepo;
        private readonly IComplaintRepository _complaintRepo;

        public CaseService(ICaseReposiotry caseRepo, IComplaintRepository complaintRepo)
        {
            _caseRepo = caseRepo;
            _complaintRepo = complaintRepo;
        }

        public async Task<Case> FullInformationAsync(long id)
        {
            return await _caseRepo.FirstOrDefaultAsync(
                filter: entity => entity.Id == id,
                include: source => source.Include(u => u.Complaint)
                                                              .ThenInclude(u => u.Complaining)
                                                        .Include(u => u.Complaint)
                                                              .ThenInclude(u => u.Plaintiff)
                                                        .Include(u => u.Complaint)
                                                            .ThenInclude(u => u.Documents)
                                                         .Include(u => u.Epistles)
                                                            .ThenInclude(u => u.Documents)
                                                         .Include(u => u.Violations)
                                                            .ThenInclude(u => u.Category)
                                                         .Include(u => u.Violations)
                                                            .ThenInclude(u => u.PrimaryVote)
                                                                .ThenInclude(u => u.Documents)
                                                         .Include(u => u.Violations)
                                                            .ThenInclude(u => u.PrimaryVote)
                                                                .ThenInclude(u => u.Verdict)
                                                         .Include(u => u.Violations)
                                                            .ThenInclude(u => u.FinalVote)
                                                                .ThenInclude(u => u.Documents)
                                                         .Include(u => u.Violations)
                                                            .ThenInclude(u => u.FinalVote)
                                                                .ThenInclude(u => u.Verdict)
                                                         .Include(u => u.Violations)
                                                            .ThenInclude(u => u.CentralCommitteeVote)
                                                                .ThenInclude(u => u.Documents)
                                                        .Include(u => u.Violations)
                                                            .ThenInclude(u => u.CentralCommitteeVote)
                                                                .ThenInclude(u => u.Verdict)
                                                        .Include(u => u.Violations)
                                                            .ThenInclude(u => u.Documents)
                                                         .Include(u => u.Informants)
                                                            .ThenInclude(u => u.Documents)
                                                         .Include(u => u.Invitations)
                                                            .ThenInclude(u => u.InvitationUsers)
                                                                .ThenInclude(u => u.User)
                                                        .Include(u => u.Invitations)
                                                            .ThenInclude(u => u.Documents)
                                                         .Include(u => u.Invitations)
                                                            .ThenInclude(u => u.Plaintiff)
                                                        .Include(u => u.Invitations)
                                                            .ThenInclude(u => u.Complaining)
                                                         .Include(u => u.Defences)
                                                            .ThenInclude(u => u.Documents)
                                                         .Include(u => u.Objections)
                                                            .ThenInclude(u => u.Documents)
                                                         .Include(u => u.RelatedInfos)
                                                            .ThenInclude(u => u.Documents));
        }

        public async Task<IEnumerable<GetCases>> GetAllAsync(Expression<Func<Case, bool>> filter = null, int skip = 0, int take = 10)
        {
            return await _caseRepo.GetAllAsync<GetCases>(
                filter: filter,
                include: source => source.Include(u => u.Complaint).ThenInclude(u => u.Complaining),
                orderBy: source => source.OrderByDescending(u => u.Complaint.CreateDate),
                skip: skip,
                take: take,
                select: entity => new GetCases
                {
                    Id = entity.Id,
                    Status = entity.Status.ToPersian(),
                    College = entity.Complaint.Complaining.College,
                    FullName = entity.Complaint.Complaining.FullName,
                    Grade = entity.Complaint.Complaining.Grade,
                    StudentNumber = entity.Complaint.Complaining.StudentNumber,
                    EducationalGroup = entity.Complaint.Complaining.EducationalGroup
                });
        }

        public async Task<Case> GetByComplaintIdAsync(long id)
        {
            return await _caseRepo.FirstOrDefaultAsync(u => u.ComplaintId == id,
                            include: source => source
                                    .Include(u => u.Complaint)
                                        .ThenInclude(u => u.Complaining)
                                    .Include(u => u.Complaint)
                                        .ThenInclude(u => u.Plaintiff));
        }

        public async Task<Case> GetByIdAsync(long id)
        {
            return await _caseRepo.FirstOrDefaultAsync(u => u.Id == id,
                            include: source => source
                                    .Include(u => u.Complaint)
                                        .ThenInclude(u => u.Complaining)
                                    .Include(u => u.Complaint)
                                        .ThenInclude(u => u.Plaintiff));
        }

        public async Task CreateAsync(CreateCase command)
        {
            var entity = new Case(command.ComplaintId);
            var complaint = await _complaintRepo.FirstOrDefaultAsync(u => u.Id == command.ComplaintId, isTracking: true);
            complaint.WithResult(ComplaintResult.SeeCase);
            _caseRepo.Add(entity);
            await _caseRepo.SaveAsync();
        }

        public long GetCount(Expression<Func<Case, bool>> filter = null)
        {
            return _caseRepo.GetCount(filter);
        }

        public async Task<bool> RemoveAsync(long id)
        {
            var entity = await _caseRepo.FirstOrDefaultAsync(u => u.Id == id,
                include: source => source.Include(u => u.Complaint));

            if (entity == null)
                return false;

            _caseRepo.Remove(entity);
            var complaint = entity.Complaint;
            complaint.WithResult(ComplaintResult.Filing);

            _complaintRepo.Update(complaint);

            await _caseRepo.SaveAsync();
            return true;
        }
    }
}
