using DisciplinarySystem.Application.Cases.Helpers;
using DisciplinarySystem.Domain.DisciplinaryCase.Cases.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DisciplinarySystem.Presentation.Controllers.Cases.ViewModels
{
	public class CaseFilters
	{
		public long Id { get; set; }
		public String StudentNumber { get; set; }
		public String NationalCode { get; set; }
		public String Name { get; set; }
		public String College { get; set; }
		public String EducationalGroup { get; set; }
		public String Grade { get; set; }
		public int Status { get; set; }
		public List<SelectListItem> StatusList { get; set; }

		public int Skip { get; set; }
		public int Take { get; set; } = 10;


		public List<SelectListItem> GetStatusList()
		{
			return new List<SelectListItem>
			{
				new SelectListItem(){Text = CaseStatus.Filing.ToPersian() , Value=$"{(int)CaseStatus.Filing}"},
				new SelectListItem(){Text = CaseStatus.Complete.ToPersian() , Value=$"{(int)CaseStatus.Complete}"},
				new SelectListItem(){Text = CaseStatus.PrimaryVote.ToPersian() , Value=$"{(int)CaseStatus.PrimaryVote}"},
				new SelectListItem(){Text = CaseStatus.Objection.ToPersian() , Value=$"{(int)CaseStatus.Objection}"},
				new SelectListItem(){Text = CaseStatus.FinalVote.ToPersian() , Value=$"{(int)CaseStatus.FinalVote}"},
				new SelectListItem(){Text = CaseStatus.CentralCommitteeVote.ToPersian() , Value=$"{(int)CaseStatus.CentralCommitteeVote}"}
			};
		}

		public bool IsEmpty()
		{
			return Id <= 0 && String.IsNullOrEmpty(StudentNumber) && String.IsNullOrEmpty(NationalCode) &&
				String.IsNullOrEmpty(Name) && String.IsNullOrEmpty(College) && String.IsNullOrEmpty(EducationalGroup)
				&& String.IsNullOrEmpty(Grade) && Status <= 0;
		}

	}
}
