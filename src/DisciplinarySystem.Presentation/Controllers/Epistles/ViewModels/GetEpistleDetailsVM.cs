using DisciplinarySystem.Domain.Epistles;

namespace DisciplinarySystem.Presentation.Controllers.Epistles.ViewModels
{
	public class GetEpistleDetailsVM
	{
		public Epistle Epistle { get; set; }
		public bool WithSteps { get; set; }
	}
}
