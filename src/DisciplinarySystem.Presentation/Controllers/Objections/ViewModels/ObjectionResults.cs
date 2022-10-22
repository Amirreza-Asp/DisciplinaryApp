using Microsoft.AspNetCore.Mvc.Rendering;

namespace DisciplinarySystem.Presentation.Controllers.Objections.ViewModels
{
    public class ObjectionResults
    {
        private static List<String> _results = new List<string>
        {
            "نامشخص",
            "رد",
            "قبول",
        };

        public static IEnumerable<SelectListItem> GetSelectResults() =>
            _results.Select(result => new SelectListItem { Text = result, Value = result });
    }
}
