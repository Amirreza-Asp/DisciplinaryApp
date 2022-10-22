using DisciplinarySystem.Domain.Informants;
using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Informants.ViewModels
{
    public class UpdateInformed
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "لطفا نام و نام خانوادگی را وارد کنید")]
        public String FullName { get; set; }

        [Required(ErrorMessage = "لطفا شماره تلفن را وارد کنید")]
        [MaxLength(11, ErrorMessage = "شماره تلفن 11 رقم است")]
        [MinLength(11, ErrorMessage = "شماره تلفن 11 رقم است")]
        public String PhoneNumber { get; set; }


        [Required(ErrorMessage = "کد ملی را وارد کنید")]
        [MaxLength(10, ErrorMessage = "کد ملی 10 رقم است")]
        [MinLength(10, ErrorMessage = "کد ملی 10 رقم است")]
        public String NationalCode { get; set; }

        [Required(ErrorMessage = "نام پدر را وارد کنید")]
        public String Father { get; set; }

        [Required(ErrorMessage = "لطفا موضوع را وارد کنید")]
        public String Subject { get; set; }

        [Required(ErrorMessage = "لطفا اظهارات را وارد کنید")]
        public String Statements { get; set; }

        public DateTime CreateDate { get; set; }
        public long CaseId { get; set; }

        public List<String>? NewDocuments { get; set; }
        public List<InformedDocumentDto>? CurrentDocuments { get; set; }


        public static UpdateInformed Create(Informed entity)
        {
            return new UpdateInformed
            {
                Id = entity.Id,
                CaseId = entity.CaseId,
                Statements = entity.Statements,
                FullName = entity.FullName,
                Subject = entity.Subject,
                CreateDate = entity.CreateDate,
                PhoneNumber = entity.PhoneNumber,
                NationalCode = entity.NationalCode,
                Father = entity.Father,
                CurrentDocuments = entity.Documents.Select(u => InformedDocumentDto.Create(u)).ToList()
            };
        }
    }
}
