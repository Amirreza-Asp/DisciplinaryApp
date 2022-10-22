using DisciplinarySystem.Application.Complaints.ViewModels.Create;
using DisciplinarySystem.Domain.Complaints;

namespace DisciplinarySystem.Application.Complaints.ViewModels.Update
{
    public class UpdateComplaining : CreateComplaining
    {
        public Guid Id { get; set; }

        public static UpdateComplaining CreateWitComplaining(Complaining complaining)
        {
            return new UpdateComplaining
            {
                Id = complaining.Id,
                College = complaining.College,
                NationalCode = complaining.NationalCode,
                StudentNumber = complaining.StudentNumber,
                EducationGroup = complaining.EducationalGroup,
                FullName = complaining.FullName,
                Grade = complaining.Grade,
                Father = complaining.Father
            };
        }
    }
}
