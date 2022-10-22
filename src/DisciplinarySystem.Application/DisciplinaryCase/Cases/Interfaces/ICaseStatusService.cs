namespace DisciplinarySystem.Application.DisciplinaryCase.Cases.Interfaces
{
    public interface ICaseStatusService
    {
        Task Fix(long caseId);
    }
}
