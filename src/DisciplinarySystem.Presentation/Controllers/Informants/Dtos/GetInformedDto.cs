using DisciplinarySystem.Domain.FinalVotes;
using DisciplinarySystem.Domain.Informants;
using DisciplinarySystem.SharedKernel.ValueObjects;

namespace DisciplinarySystem.Presentation.Controllers.Informants.Dtos
{
    public class GetInformedDto
    {
        public Guid Id { get; set; }
        public String FullName { get;  set; }
        public String PhoneNumber { get;  set; }
        public String NationalCode { get;  set; }
        public String Father { get; set; }
        public String Subject { get;  set; }
        public String Statements { get;  set; }
        public DateTime CreateDate { get;  set; }
        public IEnumerable<GetInformedDocumentDto> Documents { get; set; }

        public static GetInformedDto Create(Informed entity) =>
            new GetInformedDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                PhoneNumber = entity.PhoneNumber,
                NationalCode = entity.NationalCode,
                Father = entity.Father,
                Subject = entity.Subject,
                Statements = entity.Statements,
                CreateDate = entity.CreateDate,
                Documents = GetInformedDocumentDto.Create(entity.Documents)
            };
    }


    public class GetInformedDocumentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime SendTime { get; set; }

        public static IEnumerable<GetInformedDocumentDto> Create(IEnumerable<InformedDocument> entities) =>
            entities.Select(entity => new GetInformedDocumentDto
            {
                Id = entity.Id,
                Name = entity.Name,
                SendTime = entity.SendTime
            });
    }

}
