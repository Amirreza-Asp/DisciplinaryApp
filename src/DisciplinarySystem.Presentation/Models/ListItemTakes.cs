using Microsoft.AspNetCore.Mvc.Rendering;

namespace DisciplinarySystem.Presentation.Models
{
    public static class ListItemTakes
    {
        private static List<int> _numbers = new List<int>
        {
            5,
            10,
            25,
            50
        };

        public static IEnumerable<SelectListItem> GetSelectedNumbers() =>
            _numbers.Select(number => new SelectListItem
            {
                Text = number.ToString(),
                Value = number.ToString()
            });
    }
}
