using DisciplinarySystem.Application.Complaints.ViewModels;

namespace DisciplinarySystem.Presentation.Controllers.Complaints.ViewModels
{
	public class GetComplaintsListVM
	{
		public IEnumerable<GetComplaint> Complaints { get; set; } = new List<GetComplaint>();
		public int TotalCount { get; set; }
		public FilterVM Filters { get; set; }
	}
}
