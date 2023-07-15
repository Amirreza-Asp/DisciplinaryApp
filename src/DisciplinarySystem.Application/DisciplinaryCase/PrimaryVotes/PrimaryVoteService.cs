using DisciplinarySystem.Application.DisciplinaryCase.Cases.Interfaces;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Application.PrimaryVotes.Interfaces;
using DisciplinarySystem.Application.PrimaryVotes.ViewModels;
using DisciplinarySystem.Domain.PrimaryVotes;
using DisciplinarySystem.Domain.Verdicts;
using DisciplinarySystem.Domain.Violations;
using DisciplinarySystem.SharedKernel.Common;
using DisciplinarySystem.SharedKernel.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.PrimaryVotes
{
    public class PrimaryVoteService : IPrimaryVoteService
    {
        private readonly IRepository<PrimaryVote> _prvRepo;
        private readonly IRepository<PrimaryVoteDocument> _docRepo;
        private readonly IRepository<Verdict> _verdictRepo;
        private readonly IRepository<Violation> _vioRepo;
        private readonly ICaseReposiotry _caseRepo;
        private readonly ICaseStatusService _caseStatusService;

        public PrimaryVoteService(IRepository<PrimaryVote> prvRepo, IRepository<PrimaryVoteDocument> docRepo, IRepository<Verdict> verdictRepo, ICaseReposiotry caseRepo, ICaseStatusService caseStatusService, IRepository<Violation> vioRepo)
        {
            _prvRepo = prvRepo;
            _docRepo = docRepo;
            _verdictRepo = verdictRepo;
            _caseRepo = caseRepo;
            _caseStatusService = caseStatusService;
            _vioRepo = vioRepo;
        }

        public async Task<IEnumerable<PrimaryVote>> ListAsync(Expression<Func<PrimaryVote, bool>> filter = null, int skip = 0, int take = 10)
        {
            return await _prvRepo.GetAllAsync(
                filter: filter,
                include: source => source.Include(u => u.Verdict).Include(u => u.Violation),
                orderBy: source => source.OrderByDescending(b => b.Violation.Title).OrderByDescending(b => b.CreateTime),
                take: take,
                skip: skip);
        }

        public int GetCount(Expression<Func<PrimaryVote, bool>> filter = null) => _prvRepo.GetCount(filter);

        public async Task<IEnumerable<SelectListItem>> GetSelectedVotesAsync()
        {
            return await _verdictRepo.GetAllAsync<SelectListItem>(
                    select: entity => new SelectListItem
                    {
                        Text = entity.Title,
                        Value = entity.Id.ToString()
                    }
                );
        }

        public async Task<PrimaryVote> GetByIdAsync(Guid id)
        {
            return await _prvRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id,
                include: source => source
                .Include(u => u.Verdict)
                .Include(u => u.Violation)
                .Include(u => u.Documents));
        }

        public async Task<PrimaryVote> GetByCaseIdAsync(long caseId)
        {
            return await _prvRepo.FirstOrDefaultAsync(
                filter: u => u.Violation.CaseId == caseId,
                include: source => source.Include(u => u.Verdict)
                                .Include(u => u.Violation)
                                .Include(u => u.Documents));
        }

        public async Task<PrimaryVoteDocument> GetDocumentByIdAsync(Guid id)
        {
            return await _docRepo.FindAsync(id);
        }

        public async Task CreateAsync(CreatePrimaryVote command, IFormFileCollection files)
        {
            var entity = new PrimaryVote(command.Description, command.VerdictId, command.ViolationId, command.IsClosed);
            AddDocuments(entity.Id, files);


            var caseEntity = await _caseRepo.FirstOrDefaultAsync(u => u.Id == command.CaseId);
            if ((int)caseEntity.Status < (int)CaseStatus.PrimaryVote)
            {
                caseEntity.WithStatus(CaseStatus.PrimaryVote);
                _caseRepo.Update(caseEntity);
            }

            await UpdateViolationVote(command.CaseId, command.ViolationId, command.VerdictId, true);
            _prvRepo.Add(entity);
            _prvRepo.Save();
        }

        public async Task UpdateAsync(UpdatePrimaryVote command, IFormFileCollection files)
        {
            var entity = await _prvRepo.FindAsync(command.Id);
            if (entity == null)
                return;


            entity.WithVerdictId(command.VerdictId)
                .WithDescription(command.Description)
                .WithIsClosed(command.IsClosed)
                .WithViolationId(command.ViolationId);

            await UpdateViolationVote(command.CaseId, command.ViolationId, command.VerdictId, true);

            AddDocuments(entity.Id, files);
            _prvRepo.Update(entity);
            await _prvRepo.SaveAsync();
        }

        public async Task<bool> RemoveAsync(Guid id, long caseId)
        {
            var entity = _prvRepo.FirstOrDefault(
                filter: u => u.Id == id,
                include: source => source.Include(u => u.Documents));
            if (entity == null)
                return false;

            await UpdateViolationVote(caseId, entity.ViolationId, entity.VerdictId, false);
            entity.Documents.ToList().ForEach(doc => doc.RemoveFile());

            _prvRepo.Remove(entity);
            await _prvRepo.SaveAsync();
            await _caseStatusService.Fix(caseId);
            return true;
        }

        public async Task<bool> RemoveFileAsync(Guid id)
        {
            var doc = await _docRepo.FindAsync(id);
            if (doc == null)
                return false;

            doc.RemoveFile();
            _docRepo.Remove(doc);
            await _docRepo.SaveAsync();

            return true;
        }


        private void AddDocuments(Guid id, IFormFileCollection files)
        {
            if (files == null)
                return;

            foreach (var file in files)
            {
                var doc = new PrimaryVoteDocument(id, file.FileName, new Document(file.FileName, file.ReadBytes()));
                doc.CreateFile();
                _docRepo.Add(doc);
            }
        }
        private async Task UpdateViolationVote(long caseId, Guid vioId, long verdictId, bool addOrEdit)
        {
            var violations = await _vioRepo.GetAllAsync(
                u => u.CaseId == caseId,
                include: source =>
                    source.Include(u => u.CentralCommitteeVote)
                            .Include(u => u.FinalVote));

            if (violations.Any(vio => vio.CentralCommitteeVote != null || vio.FinalVote != null))
                return;

            foreach (var vio in violations)
            {
                if (vio.Id != vioId || !addOrEdit)
                    vio.WithVote(null);
                else
                {
                    var verdict = await _verdictRepo.FindAsync(verdictId);
                    vio.WithVote(verdict.Title);
                }
                _vioRepo.Update(vio);
            }
        }

    }
}
