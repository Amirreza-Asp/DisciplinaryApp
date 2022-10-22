using Microsoft.AspNetCore.Mvc.Rendering;

namespace DisciplinarySystem.Presentation.Models
{
    public class SearchSelectBox
    {
        public String For { get; set; }

        public IEnumerable<SelectListItem>? Items { get; set; }
    }
}
