using DisciplinarySystem.Domain.RelatedInfos;

namespace DisciplinarySystem.Application.RelatedInfos.ViewModels
{
    public class UpdateRelatedInfo : CreateRelatedInfo
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public IEnumerable<RelatedInfoDocument>? CurrentDocuments { get; set; }

        public static UpdateRelatedInfo Create(RelatedInfo entity) =>
            new UpdateRelatedInfo
            {
                CaseId = entity.CaseId,
                CreateDate = entity.CreateDate,
                Description = entity.Description,
                Id = entity.Id,
                Subject = entity.Subject,
                CurrentDocuments = entity.Documents
            };
    }
}
