using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Invitations.ViewModels
{
    public class CreateInvitation
    {
        [Required(ErrorMessage = "عنوان را وارد کنید")]
        public String Subject { get; set; }

        [Required(ErrorMessage = "شرح را وارد کنید")]
        [MaxLength(50 , ErrorMessage = "شرح دعوت باید کمتر از 50 کاراکتر باشد")]
        public String Description { get; set; }

        [Required(ErrorMessage = "تاریخ دعوت را وارد کنید")]
        public String InviteDate { get; set; }

        public long CaseId { get; set; }

        [Required(ErrorMessage = "مدعوین را اضافه کنید")]
        public String PersonsId { get; set; }

        public List<InvitationsInfo>? SelectedPersons { get; set; }

        public List<SelectListItem>? Persons { get; set; }
        public List<String> Documents { get; set; }
    }
}
