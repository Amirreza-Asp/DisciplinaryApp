using DisciplinarySystem.Domain.DisciplinaryCase.Cases;
using DisciplinarySystem.Domain.DisciplinaryCase.CentralCommitteeVotes;
using DisciplinarySystem.Domain.FinalVotes;
using DisciplinarySystem.Domain.PrimaryVotes;
using DisciplinarySystem.Domain.RelatedInfos;
using DisciplinarySystem.Domain.Violations;

namespace DisciplinarySystem.Presentation.Controllers.Violations.Dtos
{
    public class GetViolationDto
    {
        public Guid Id { get; set; }
        public string Title { get;  set; }
        public string Definition { get;  set; }
        public DateTime CreateDate { get;  set; }
        public DateTime? UpdateDate { get; set; }
        public String? Vote { get;  set; }

        public String Category { get;  set; }
        public String? FinalVote { get;  set; }
        public String? PrimaryVote { get;  set; }
        public String? CentralCommitteeVote { get;  set; }
        public IEnumerable<GetViolationDocumentDto> Documents { get;  set; }

        public static GetViolationDto Create(Violation entity) =>
            new GetViolationDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Definition = entity.Definition,
                CreateDate = entity.CreateDate,
                UpdateDate = entity.UpdateDate,
                Vote = entity.Vote,
                Category = entity.Category.Title,
                FinalVote = entity.FinalVote == null ? null : entity.FinalVote.Verdict.Title,
                PrimaryVote = entity.PrimaryVote == null ? null : entity.PrimaryVote.Verdict.Title,
                CentralCommitteeVote = entity.CentralCommitteeVote == null ? null : entity.CentralCommitteeVote.Verdict.Title,
                Documents = GetViolationDocumentDto.Create(entity.Documents)
            };

        public class GetViolationDocumentDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public DateTime SendTime { get; set; }

            public static IEnumerable<GetViolationDocumentDto> Create(IEnumerable<ViolationDocument> entities) =>
                entities.Select(entity => new GetViolationDocumentDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    SendTime = entity.SendTime
                });
        }
    }

}
