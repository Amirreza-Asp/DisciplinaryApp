using Microsoft.AspNetCore.Mvc.Rendering;

namespace DisciplinarySystem.Presentation.Controllers.Epistles.Helpers
{
    public static class PositionList
    {
        private static List<String> _positions = new List<string>
        {
            "مدیریت",
            "دانشجو"
        };

        public static List<SelectListItem> GetPositions () =>
            _positions.Select(u => new SelectListItem
            {
                Text = u ,
                Value = u
            }).ToList();
    }
}
