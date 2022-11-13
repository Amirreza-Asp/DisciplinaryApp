using Microsoft.AspNetCore.Mvc.Rendering;

namespace DisciplinarySystem.Application.Authentication.Dtos
{
    public class HandAdd
    {
        public String Name { get; set; }
        public String Family { get; set; }
        public String Type { get; set; }
        public long Access { get; set; }
        public String PhoneNumber { get; set; }
        public String NationalCode { get; set; }
        public Guid RoleId { get; set; }


        public IEnumerable<SelectListItem>? Roles { get; set; }
        public IEnumerable<SelectListItem>? AuthRoles { get; set; }

        public List<SelectListItem> GetTypes () =>
            new List<SelectListItem>
            {
                new SelectListItem{Text= "بدوی" , Value = "بدوی"},
                new SelectListItem{Text= "تجدید نظر" , Value = "تجدید  نظر"},
            };
    }
}
