namespace DisciplinarySystem.Application.Cases.Helpers
{
	public static class CaseStatusConvertor
	{
		public static String ToPersian(this CaseStatus status) => status switch
		{
			CaseStatus.Filing => "تشکیل پرونده",
			CaseStatus.Complete => "تکمیل پرونده",
			CaseStatus.PrimaryVote => "صدور رای اولیه",
			CaseStatus.Objection => "بررسی اعتراض",
			CaseStatus.FinalVote => "صدور حکم نهایی",
			CaseStatus.CentralCommitteeVote => "صدور حکم مرکزی ",
			_ => "نامشخص"
		};
	}
}
