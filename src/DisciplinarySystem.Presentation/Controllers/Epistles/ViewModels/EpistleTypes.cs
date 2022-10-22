using Microsoft.AspNetCore.Mvc.Rendering;

namespace DisciplinarySystem.Presentation.Controllers.Epistles.ViewModels
{
    public static class EpistleTypes
    {
        private static List<String> _types = new List<string>
        {
            "حکم",
            "دستورالعمل",
            "بخشنامه",
            "هماهنگی",
            "اطلاعیه",
            "درون سازمانی",
            "برون سازمانی",
            "فوری",
            "محرمانه",
        };

        public static IEnumerable<SelectListItem> GetSelectedTypes() => _types.Select(type => new SelectListItem
        {
            Text = type,
            Value = type
        });
    }
}
