using DisciplinarySystem.Domain.DisciplinaryCase.Cases;

namespace DisciplinarySystem.Presentation.Controllers.Cases.ViewModels
{
    public class CaseDetails
	{
		public Case Case { get; set; }
		public bool OnlySee { get; set; }
	}
}
