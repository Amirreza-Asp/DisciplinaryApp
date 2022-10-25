namespace DisciplinarySystem.Application.DisciplinaryCase.Complaints.ViewModels.Update
{
    public class UpdateResult
    {
        public UpdateComplaintResult Result { get; private set; }

        public UpdateResult ()
        {
            Result = UpdateComplaintResult.NotChanged;
        }

        public void CaseList ()
        {
            Result = UpdateComplaintResult.CaseList;
        }

        public UpdateComplaintResult ShowResult () => Result;
    }

    public enum UpdateComplaintResult
    {
        CaseList,
        NotChanged
    }
}
