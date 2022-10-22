using DisciplinarySystem.Application.DisciplinaryCase.Cases.Interfaces;
using DisciplinarySystem.Application.FinalVotes.Interfaces;
using DisciplinarySystem.Application.FinalVotes.ViewModels;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Domain.FinalVotes;
using DisciplinarySystem.Domain.Verdicts;
using DisciplinarySystem.Domain.Violations;
using DisciplinarySystem.SharedKernel.Common;
using DisciplinarySystem.SharedKernel.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.FinalVotes
{
    public class FinalVoteService : IFinalVoteService
    {
        private readonly IRepository<FinalVote> _fvoRepo;
        private readonly IRepository<FinalVoteDocument> _docRepo;
        private readonly IRepository<Verdict> _verdictRepo;
        private readonly IRepository<Violation> _violationRepo;
        private readonly ICaseReposiotry _caseRepo;
        private readonly ICaseStatusService _caseStatusService;

        public FinalVoteService ( IRepository<FinalVote> fvoRepo , IRepository<FinalVoteDocument> docRepo , IRepository<Verdict> verdictRepo ,
            IRepository<Violation> violationRepo , ICaseReposiotry caseRepo , ICaseStatusService caseStatusService )
        {
            _fvoRepo = fvoRepo;
            _docRepo = docRepo;
            _verdictRepo = verdictRepo;
            _violationRepo = violationRepo;
            _caseRepo = caseRepo;
            _caseStatusService = caseStatusService;
        }

        public async Task<IEnumerable<FinalVote>> ListAsync ( Expression<Func<FinalVote , bool>> filter = null , int skip = 0 , int take = 10 )
        {
            return await _fvoRepo.GetAllAsync(
                filter: filter ,
                include: source => source.Include(u => u.Verdict)
                                                        .Include(u => u.Violation) ,
                orderBy: source => source.OrderByDescending(b => b.CreateTime) ,
                take: take ,
                skip: skip);
        }

        public async Task<FinalVoteDocument> GetDocumentByIdAsync ( Guid id )
        {
            return await _docRepo.FindAsync(id);
        }

        public async Task<FinalVote> GetByCaseIdAsync ( long caseId )
        {
            return await _fvoRepo.FirstOrDefaultAsync(
                filter: u => u.Violation.CaseId == caseId ,
                include: source =>
                    source.Include(u => u.Verdict)
                    .Include(u => u.Violation)
                    .Include(u => u.Documents));
        }

        public int GetCount ( Expression<Func<FinalVote , bool>> filter = null ) => _fvoRepo.GetCount(filter);

        public async Task<IEnumerable<SelectListItem>> GetSelectedVotesAsync ()
        {
            return await _verdictRepo.GetAllAsync<SelectListItem>(
                    select: entity => new SelectListItem
                    {
                        Text = entity.Title ,
                        Value = entity.Id.ToString()
                    }
                );
        }

        public async Task<FinalVote> GetByIdAsync ( Guid id )
        {
            return await _fvoRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source
                .Include(u => u.Violation)
                .Include(u => u.Verdict)
                .Include(u => u.Documents));
        }


        public async Task CreateAsync ( CreateFinalVote command , IFormFileCollection files )
        {
            var entity = new FinalVote(command.Description , command.VerdictId , command.ViolationId);
            AddDocuments(entity.Id , files);

            var caseEntity = await _caseRepo.FirstOrDefaultAsync(u => u.Id == command.CaseId);
            if ( caseEntity == null )
                return;

            if ( caseEntity.Status < CaseStatus.FinalVote )
            {
                caseEntity.WithStatus(CaseStatus.FinalVote);
                _caseRepo.Update(caseEntity);
            }

            await AddViolationVote(command.CaseId , command.ViolationId , command.VerdictId);
            _fvoRepo.Add(entity);
            await _fvoRepo.SaveAsync();
        }

        public async Task UpdateAsync ( UpdateFinalVote command , IFormFileCollection files )
        {
            var entity = await _fvoRepo.FindAsync(command.Id);
            if ( entity == null )
                return;

            entity.WithVerdictId(command.VerdictId)
                .WithDescription(command.Description)
                .WithViolationId(command.ViolationId);

            AddDocuments(entity.Id , files);

            await AddViolationVote(command.CaseId , command.ViolationId , command.VerdictId);

            _fvoRepo.Update(entity);
            await _fvoRepo.SaveAsync();
        }

        public async Task<bool> RemoveAsync ( Guid id , long caseId )
        {
            var entity = _fvoRepo.FirstOrDefault(
                filter: u => u.Id == id ,
                include: source => source.Include(u => u.Documents));

            if ( entity == null )
                return false;

            entity.Documents.ToList().ForEach(doc => doc.RemoveFile());
            _fvoRepo.Remove(entity);

            await RemoveViolationVote(caseId);

            await _fvoRepo.SaveAsync();
            await _caseStatusService.Fix(caseId);

            return true;
        }

        public async Task<bool> RemoveFileAsync ( Guid id )
        {
            var doc = await _docRepo.FindAsync(id);
            if ( doc == null )
                return false;

            doc.RemoveFile();
            _docRepo.Remove(doc);
            await _docRepo.SaveAsync();

            return true;
        }


        private void AddDocuments ( Guid id , IFormFileCollection files )
        {
            if ( files == null )
                return;

            foreach ( var file in files )
            {
                var doc = new FinalVoteDocument(id , file.FileName , new Document(file.FileName , file.ReadBytes()));
                doc.CreateFile();
                _docRepo.Add(doc);
            }
        }
        private async Task AddViolationVote ( long caseId , Guid vioId , long verdictId )
        {
            var violations = await _violationRepo.GetAllAsync(
                filter: u => u.CaseId == caseId ,
                include: source =>
                        source.Include(u => u.CentralCommitteeVote));

            if ( violations.Any(u => u.CentralCommitteeVote != null) )
                return;

            foreach ( var vio in violations )
            {
                if ( vio.Id == vioId )
                {
                    var verdict = await _verdictRepo.FindAsync(verdictId);
                    vio.WithVote(verdict.Title);
                }
                else
                    vio.WithVote(null);

                _violationRepo.Update(vio);
            }
        }
        private async Task RemoveViolationVote ( long caseId )
        {
            var violations = await _violationRepo.GetAllAsync(
                filter: u => u.CaseId == caseId ,
                include: source => source
                            .Include(u => u.CentralCommitteeVote)
                                .ThenInclude(u => u.Verdict)
                            .Include(u => u.PrimaryVote)
                                .ThenInclude(u => u.Verdict) ,
                isTracking: true);

            if ( violations.Any(u => u.CentralCommitteeVote != null) )
                return;

            foreach ( var vio in violations )
            {
                if ( vio.PrimaryVote != null )
                    vio.WithVote(vio.PrimaryVote.Verdict.Title);
                else
                    vio.WithVote(null);
            }
        }
    }
}
