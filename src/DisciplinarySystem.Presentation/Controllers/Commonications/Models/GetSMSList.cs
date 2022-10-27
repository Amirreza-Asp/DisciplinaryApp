using DisciplinarySystem.Domain.Commonications;

namespace DisciplinarySystem.Presentation.Controllers.Commonications.Models
{
    public class GetSMSList
    {
        public IEnumerable<SMS> Entities { get; set; }
        public SMSFilter Filters { get; set; }
    }
}
