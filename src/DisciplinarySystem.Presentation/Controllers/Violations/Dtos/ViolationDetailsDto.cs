using DisciplinarySystem.Domain.Violations;

namespace DisciplinarySystem.Presentation.Controllers.Violations.Dtos
{
    public class ViolationDetailsDto
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public String? Vote { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public static ViolationDetailsDto Create(Violation entity) =>
            new ViolationDetailsDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Vote = entity.Vote,
                CreateDate = entity.CreateDate,
                UpdateDate = entity.UpdateDate
            };
    }
}
