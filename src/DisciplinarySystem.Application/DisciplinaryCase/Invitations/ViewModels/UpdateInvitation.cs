using DisciplinarySystem.Domain.Invitations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Invitations.ViewModels
{
    public class UpdateInvitation
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "عنوان را وارد کنید")]
        public String Subject { get; set; }

        [Required(ErrorMessage = "شرح را وارد کنید")]
        public String Description { get; set; }

        public long CaseId { get; set; }

        [Required(ErrorMessage = "تاریخ دعوت را وارد کنید")]
        public String InviteDate { get; set; }

        public String? NewPersonsId { get; set; }

        public List<InvitationsInfo>? SelectedPersons { get; set; }
        public DateTime CreateTime { get; set; }

        public List<SelectListItem>? Persons { get; set; }
        public List<InvitationDocument>? CurrentDocuments { get; set; }
        public List<String>? NewDocuments { get; set; }



        public static UpdateInvitation Create(Invitation entity)
        {
            var obj = new UpdateInvitation
            {
                Id = entity.Id,
                CaseId = entity.CaseId,
                Description = entity.Description,
                Subject = entity.Subject,
                CurrentDocuments = entity.Documents.ToList(),
                CreateTime = entity.CreateDate,
                // 2025/3/12 12:53
                InviteDate = entity.InviteDate.Year + "/" + entity.InviteDate.Month + "/" + entity.InviteDate.Day + " " + entity.InviteDate.TimeOfDay.ToString().Substring(0, 5),
                SelectedPersons = entity.InvitationUsers.Select(u => new InvitationsInfo
                {
                    Id = u.UserId,
                    Name = u.User.FullName,
                    Optgroup = "اعضای کمیته"
                }).ToList()
            };

            if (entity.ComplainingId.HasValue)
            {
                obj.SelectedPersons.Insert(0, new InvitationsInfo
                {
                    Id = entity.ComplainingId.Value,
                    Name = entity.Complaining.FullName,
                    Optgroup = "متشاکی"
                });
            }

            if (entity.PlaintiffId.HasValue)
            {
                obj.SelectedPersons.Insert(0, new InvitationsInfo
                {
                    Id = entity.PlaintiffId.Value,
                    Name = entity.Plaintiff.FullName,
                    Optgroup = "شاکی"
                });
            }

            return obj;
        }
    }
}
