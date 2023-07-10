using DisciplinarySystem.Domain.Complaints;
using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Cases.ViewModels
{
    public class CreateCase
    {
        [Required(ErrorMessage = "شماره پرونده را وارد کنید")]
        [Range(0, long.MaxValue, ErrorMessage = "شماره پرونده نمیتواند منفی باشد")]
        public long Id { get; set; }

        [Required(ErrorMessage = "لطفا شماره شکایت را وارد کنید")]
        public long ComplaintId { get; set; }

        public String ComplainingStudentId { get; set; }
        public String ComplainingNationalCode { get; set; }
        public String CompliningFullName { get; set; }
        public String ComplainingEducationGroup { get; set; }
        public String ComplainingCollege { get; set; }
        public String ComplainingGrade { get; set; }

        public String PlaintiffFullName { get; set; }
        public String PlaintiffNationalCode { get; set; }
        public String PlaintiffPhoneNumber { get; set; }



        public static CreateCase Create(Complaint complaint)
        {
            return new CreateCase
            {
                ComplaintId = complaint.Id,

                ComplainingNationalCode = complaint.Complaining.NationalCode,
                ComplainingCollege = complaint.Complaining.College,
                ComplainingEducationGroup = complaint.Complaining.EducationalGroup,
                ComplainingStudentId = complaint.Complaining.StudentNumber,
                ComplainingGrade = complaint.Complaining.Grade,
                CompliningFullName = complaint.Complaining.FullName,

                PlaintiffFullName = complaint.Plaintiff.FullName,
                PlaintiffNationalCode = complaint.Plaintiff.NationalCode,
                PlaintiffPhoneNumber = complaint.Plaintiff.PhoneNumber
            };
        }

    }
}
