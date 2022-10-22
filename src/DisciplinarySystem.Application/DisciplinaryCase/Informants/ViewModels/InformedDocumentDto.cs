using DisciplinarySystem.Domain.Informants;

namespace DisciplinarySystem.Application.Informants.ViewModels
{
    public class InformedDocumentDto
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String File { get; set; }
        public DateTime CreateDate { get; set; }

        public static InformedDocumentDto Create(InformedDocument informedDocument)
        {
            return new InformedDocumentDto
            {
                Id = informedDocument.Id,
                File = informedDocument.File.Name,
                Name = informedDocument.Name,
                CreateDate = informedDocument.SendTime
            };
        }
    }
}
