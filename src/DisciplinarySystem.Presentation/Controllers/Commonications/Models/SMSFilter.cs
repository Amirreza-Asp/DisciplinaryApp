using DisciplinarySystem.Presentation.Models;

namespace DisciplinarySystem.Presentation.Controllers.Commonications.Models
{
    public class SMSFilter
    {
        // pagenation
        public int Skip { get; set; }
        public int Take { get; set; } = 10;
        public int Total { get; set; }

        public Pagenation ToPagenation () => Pagenation.Create(Skip , Take , Total);
    }
}
