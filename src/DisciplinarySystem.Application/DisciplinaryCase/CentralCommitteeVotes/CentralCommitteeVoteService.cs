using DisciplinarySystem.Application.DisciplinaryCase.Cases.Interfaces;
using DisciplinarySystem.Application.DisciplinaryCase.CentralCommitteeVotes.Interfaces;
using DisciplinarySystem.Application.DisciplinaryCase.CentralCommitteeVotes.ViewModels;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Domain.DisciplinaryCase.CentralCommitteeVotes;
using DisciplinarySystem.Domain.Verdicts;
using DisciplinarySystem.Domain.Violations;
using DisciplinarySystem.SharedKernel.Common;
using DisciplinarySystem.SharedKernel.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.DisciplinaryCase.CentralCommitteeVotes
{
    public class CentralCommitteeVoteService : ICentralCommitteeVoteService
    {

        private readonly IRepository<CentralCommitteeVote> _ccvRepo;
        private readonly IRepository<CentralCommitteeVoteDocument> _docRepo;
        private readonly ICaseReposiotry _caseRepo;
        private readonly ICaseStatusService _caseStatusService;
        private readonly IRepository<Violation> _violationRepo;
        private readonly IRepository<Verdict> _verdictRepo;

        public CentralCommitteeVoteService ( IRepository<CentralCommitteeVote> ccvRepo , IRepository<CentralCommitteeVoteDocument> docRepo ,
            ICaseReposiotry caseRepo , ICaseStatusService caseStatusService , IRepository<Violation> violationRepo , IRepository<Verdict> verdictRepo )
        {
            _ccvRepo = ccvRepo;
            _docRepo = docRepo;
            _caseRepo = caseRepo;
            _caseStatusService = caseStatusService;
            _violationRepo = violationRepo;
            _verdictRepo = verdictRepo;
        }


        public async Task<CentralCommitteeVote> GetByCaseIdAsync ( long id )
        {
            return await _ccvRepo.FirstOrDefaultAsync(
                filter: u => u.Violation.CaseId == id ,
                include: source => source.Include(u => u.Verdict)
                                                    .Include(u => u.Violation)
                                                    .Include(u => u.Documents));
        }

        public async Task<IEnumerable<CentralCommitteeVote>> ListAsync ( Expression<Func<CentralCommitteeVote , bool>> filter = null , int skip = 0 , int take = 10 )
        {
            return await _ccvRepo.GetAllAsync(
                filter: filter ,
                include: source => source.Include(u => u.Verdict).Include(u => u.Violation) ,
                orderBy: source => source.OrderByDescending(u => u.Violation.Title)
                .OrderByDescending(b => b.CreateTime) ,
                take: take ,
                skip: skip);
        }

        public int GetCount ( Expression<Func<CentralCommitteeVote , bool>> filter = null ) => _ccvRepo.GetCount(filter);

        public async Task<CentralCommitteeVote> GetByIdAsync ( Guid id )
        {
            return await _ccvRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source
                .Include(u => u.Verdict)
                .Include(u => u.Violation)
                .Include(u => u.Documents));
        }

        public async Task<CentralCommitteeVoteDocument> GetDocumentByIdAsync ( Guid id )
        {
            return await _docRepo.FindAsync(id);
        }

        public async Task CreateAsync ( CreateCentralCommitteeVote command , IFormFileCollection files )
        {
            var entity = new CentralCommitteeVote(command.Description , command.VerdictId , command.ViolationId);
            AddDocuments(entity.Id , files);

            var caseEntity = await _caseRepo.FindAsync(command.CaseId);
            if ( ( int ) caseEntity.Status < ( int ) CaseStatus.CentralCommitteeVote )
            {
                caseEntity.WithStatus(CaseStatus.CentralCommitteeVote);
                _caseRepo.Update(caseEntity);
            }

            await AddViolationVote(command.CaseId , command.ViolationId , command.VerdictId);
            _ccvRepo.Add(entity);

            await _ccvRepo.SaveAsync();
        }

        public async Task UpdateAsync ( UpdateCentralCommitteeVote command , IFormFileCollection files )
        {
            var entity = await _ccvRepo.FindAsync(command.Id);
            if ( entity == null )
                return;


            entity.WithVerdictId(command.VerdictId)
                .WithDescription(command.Description)
                .WithViolationId(command.ViolationId);

            AddDocuments(entity.Id , files);

            _ccvRepo.Update(entity);
            await AddViolationVote(command.CaseId , command.ViolationId , command.VerdictId);
            await _ccvRepo.SaveAsync();
        }

        public async Task<bool> RemoveAsync ( Guid id , long caseId )
        {
            var entity = _ccvRepo.FirstOrDefault(
                filter: u => u.Id == id ,
                include: source => source.Include(u => u.Documents));

            if ( entity == null )
                return false;

            entity.Documents.ToList().ForEach(doc => doc.RemoveFile());
            _ccvRepo.Remove(entity);

            await RemoveViolationVote(caseId);
            await _ccvRepo.SaveAsync();

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
                var doc = new CentralCommitteeVoteDocument(id , file.FileName , new Document(file.FileName , file.ReadBytes()));
                doc.CreateFile();
                _docRepo.Add(doc);
            }
        }
        private async Task AddViolationVote ( long caseId , Guid vioId , long verdictId )
        {
            var violations = await _violationRepo.GetAllAsync(filter: u => u.CaseId == caseId);

            foreach ( var vio in violations )
            {
                vio.WithVote(null);
                if ( vio.Id == vioId )
                {
                    var verdict = await _verdictRepo.FindAsync(verdictId);
                    vio.WithVote(verdict.Title);
                }
                _violationRepo.Update(vio);
            }
        }
        private async Task RemoveViolationVote ( long caseId )
        {
            var violations = await _violationRepo.GetAllAsync(
                filter: u => u.CaseId == caseId ,
                include: source => source
                        .Include(u => u.PrimaryVote)
                            .ThenInclude(u => u.Verdict)
                        .Include(u => u.FinalVote)
                            .ThenInclude(u => u.Verdict) ,
                isTracking: true);


            foreach ( var vio in violations )
            {
                if ( vio.FinalVote != null )
                    vio.WithVote(vio.FinalVote.Verdict.Title);
                else if ( vio.PrimaryVote != null )
                    vio.WithVote(vio.PrimaryVote.Verdict.Title);
                else
                    vio.WithVote(null);
            }
        }

    }
}
