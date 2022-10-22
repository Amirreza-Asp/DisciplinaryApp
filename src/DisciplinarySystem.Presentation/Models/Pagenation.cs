namespace DisciplinarySystem.Presentation.Models
{
	public class Pagenation
	{
		private Pagenation() { }

		public int TotalCount { get; set; }
		public int Skip { get; set; }
		public int Take { get; set; }

		public static Pagenation Create(int skip, int take, int totalCount) =>
			 new Pagenation
			 {
				 Skip = Math.Max(0, skip),
				 Take = Math.Max(0, take),
				 TotalCount = Math.Max(0, totalCount)
			 };
	}
}
