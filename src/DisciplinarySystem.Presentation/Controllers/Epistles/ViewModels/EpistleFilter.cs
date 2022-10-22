namespace DisciplinarySystem.Presentation.Controllers.Epistles.ViewModels
{
	public class EpistleFilter
	{
		public long Id { get; set; }
		public DateTime CreateDate { get; set; }
		public String Type { get; set; }
		public String Subject { get; set; }
		public String Sender_Reciver { get; set; }
		public long ComplaintId { get; set; }
		public long CaseId { get; set; }
		public bool OnlySee { get; set; }

		public int Skip { get; set; }
		public int Take { get; set; } = 10;


		public bool IsEmpty()
		{
			return Id <= 0 && CreateDate == default && String.IsNullOrEmpty(Type) && String.IsNullOrEmpty(Subject)
				&& String.IsNullOrEmpty(Sender_Reciver) && ComplaintId <= 0 && CaseId <= 0;
		}
	}
}
