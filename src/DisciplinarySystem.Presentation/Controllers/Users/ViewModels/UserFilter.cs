using Microsoft.AspNetCore.Mvc.Rendering;

namespace DisciplinarySystem.Presentation.Controllers.Users.ViewModels
{
    public class UserFilter
    {
        public String FullName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid? RoleId { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; } = 10;
        public List<SelectListItem>? Roles { get; set; }


        public bool IsEmpty() => String.IsNullOrEmpty(FullName) && StartDate == default && EndDate == default && !RoleId.HasValue;
    }
}
