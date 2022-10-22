using Microsoft.AspNetCore.Mvc.Rendering;

namespace DisciplinarySystem.Presentation.Controllers.FinalVotes.ViewModels
{
    public class FinalVoteFilter
    {
        public Guid? ViolationId { get; set; }
        public long? VerdictId { get; set; }
        public DateTime CreateTime { get; set; }
        public bool OnlySee { get; set; }



        public IEnumerable<SelectListItem>? Verdicts { get; set; }
        public IEnumerable<SelectListItem>? Violations { get; set; }
        public long CaseId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; } = 10;


        public bool IsEmpty() => !ViolationId.HasValue && !VerdictId.HasValue && CreateTime == default;
    }
}
