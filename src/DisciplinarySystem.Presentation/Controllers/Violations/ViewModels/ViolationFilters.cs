using Microsoft.AspNetCore.Mvc.Rendering;

namespace DisciplinarySystem.Presentation.Controllers.Violations.ViewModels
{
	public class ViolationFilters
	{
		public Guid Category { get; set; }
		public String Title { get; set; }
		public DateTime CreateDate { get; set; }
		public long CaseId { get; set; }
		public bool OnlySee { get; set; }

		public int Skip { get; set; }
		public int Take { get; set; } = 10;



		public List<SelectListItem> Categories { get; set; }

		public bool IsEmpty()
		{
			return Category == default && String.IsNullOrEmpty(Title) && CreateDate == default;
		}
	}
}
