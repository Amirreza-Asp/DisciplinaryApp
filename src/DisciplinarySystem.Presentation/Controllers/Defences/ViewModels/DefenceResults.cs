using Microsoft.AspNetCore.Mvc.Rendering;

namespace DisciplinarySystem.Presentation.Controllers.Defences.ViewModels
{
	public static class DefenceResults
	{
		private static List<String> _results = new List<string>
		{
			"نتیحه 1",
			"نتیحه 2",
			"نتیحه 3",
			"نتیحه 4",
			"نتیحه 5"
		};

		public static IEnumerable<SelectListItem> GetSelectResults() =>
			_results.Select(result => new SelectListItem { Text = result, Value = result });
	}
}
