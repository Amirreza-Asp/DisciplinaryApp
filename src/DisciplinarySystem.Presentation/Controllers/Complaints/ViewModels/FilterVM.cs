namespace DisciplinarySystem.Presentation.Controllers.Complaints.ViewModels
{
	public class FilterVM
	{
		public String StudentNumber { get; set; } = "";
		public String NationalCode { get; set; } = "";
		public String FullName { get; set; } = "";
		public String EducationalGroup { get; set; } = "";
		public String College { get; set; } = "";
		public String Grade { get; set; } = "";
		public long CaseId { get; set; }
		public bool OnlySee { get; set; }

		public int Skip { get; set; } = 0;
		public int Take { get; set; } = 10;



		public bool isEmpty()
		{
			return String.IsNullOrEmpty(StudentNumber) && String.IsNullOrEmpty(NationalCode) && String.IsNullOrEmpty(FullName)
				&& String.IsNullOrEmpty(EducationalGroup) && String.IsNullOrEmpty(College) && String.IsNullOrEmpty(Grade);
		}
	}
}
