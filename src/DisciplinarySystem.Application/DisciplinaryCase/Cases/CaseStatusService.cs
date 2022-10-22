using DisciplinarySystem.Application.DisciplinaryCase.Cases.Interfaces;

namespace DisciplinarySystem.Application.DisciplinaryCase.Cases
{
    public class CaseStatusService : ICaseStatusService
    {
        private readonly ICaseReposiotry _caseRepo;

        public CaseStatusService(ICaseReposiotry caseRepo)
        {
            _caseRepo = caseRepo;
        }

        public async Task Fix(long caseId)
        {
            var caseEntity = await _caseRepo.FirstOrDefaultAsync(u => u.Id == caseId, isTracking: true);
            if (caseEntity == null)
                return;

            if (_caseRepo.IsExist(u => u.Id == caseId && u.Violations.Any(u => u.CentralCommitteeVote != null)))
                caseEntity.WithStatus(CaseStatus.CentralCommitteeVote);
            else if (_caseRepo.IsExist(u => u.Id == caseId && u.Violations.Any(u => u.FinalVote != null)))
                caseEntity.WithStatus(CaseStatus.FinalVote);
            else if (_caseRepo.IsExist(u => u.Id == caseId && u.Objections.Any()))
                caseEntity.WithStatus(CaseStatus.Objection);
            else if (_caseRepo.IsExist(u => u.Id == caseId && u.Violations.Any(u => u.PrimaryVote != null)))
                caseEntity.WithStatus(CaseStatus.PrimaryVote);
            else if (_caseRepo.IsExist(u => u.Id == caseId && (u.Epistles.Any() || u.Violations.Any() || u.Informants.Any() || u.Invitations.Any() || u.Defences.Any())))
                caseEntity.WithStatus(CaseStatus.Complete);
            else
                caseEntity.WithStatus(CaseStatus.Filing);

            await _caseRepo.SaveAsync();
        }
    }
}
