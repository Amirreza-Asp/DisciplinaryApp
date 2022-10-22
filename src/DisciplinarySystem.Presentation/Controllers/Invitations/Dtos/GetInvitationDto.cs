using DisciplinarySystem.Domain.Complaints;
using DisciplinarySystem.Domain.DisciplinaryCase.Cases;
using DisciplinarySystem.Domain.Informants;
using DisciplinarySystem.Domain.Invitations;
using DisciplinarySystem.Domain.Users;
using DisciplinarySystem.Presentation.Controllers.Complaints.ViewModels;

namespace DisciplinarySystem.Presentation.Controllers.Invitations.Dtos
{
    public class GetInvitationDto
    {
        public Guid Id { get; set; }
        public String Subject { get;  set; }
        public String Description { get;  set; }
        public long CaseId { get;  set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime InviteDate { get; set; }

        public GetComplainingApi? Complaining { get;  set; }
        public GetPlaintiffApi? Plaintiff { get;  set; }
        public IEnumerable<GetUserDto> Users { get;  set; }
        public IEnumerable<GetInvitationDocumentDto> Documents { get;  set; }

        public static GetInvitationDto Create(Invitation entity) =>
            new GetInvitationDto
            {
                Id = entity.Id,
                Subject = entity.Subject,
                CreateDate = entity.CreateDate,
                Description = entity.Description,
                CaseId = entity.CaseId,
                InviteDate = entity.InviteDate,
                Complaining = entity.Complaining != null ? GetComplainingApi.Create(entity.Complaining) : null,
                Plaintiff = entity.Plaintiff != null ? GetPlaintiffApi.Create(entity.Plaintiff) : null,
                UpdateDate = entity.UpdateDate,
                Documents = GetInvitationDocumentDto.Create(entity.Documents),
                Users = GetUserDto.Create(entity.InvitationUsers.Select(u=>u.User))
            };
    }

    public class GetInvitationDocumentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime SendTime { get; set; }

        public static IEnumerable<GetInvitationDocumentDto> Create(IEnumerable<InvitationDocument> entities) =>
            entities.Select(entity => new GetInvitationDocumentDto
            {
                Id = entity.Id,
                Name = entity.Name,
                SendTime = entity.SendTime
            });
    }

    public class GetUserDto
    {
        public Guid Id { get; set; }
        public String FullName { get; set; }
        public String Role { get; set; }

        public static IEnumerable<GetUserDto> Create(IEnumerable<User> entities) =>
             entities.Select(entity => new GetUserDto
            {
                 Id = entity.Id,
                FullName = entity.FullName,
                Role = entity.Role.Title
            });
    }
}
