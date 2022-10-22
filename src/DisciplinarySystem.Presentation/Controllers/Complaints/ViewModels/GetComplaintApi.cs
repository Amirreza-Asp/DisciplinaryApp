using DisciplinarySystem.Application.Complaints.Helpers;
using DisciplinarySystem.Domain.Complaints;
using DisciplinarySystem.Domain.Complaints.Enums;
using DisciplinarySystem.Domain.DisciplinaryCase.Cases;
using DisciplinarySystem.Domain.Epistles;
using DisciplinarySystem.SharedKernel.ValueObjects;

namespace DisciplinarySystem.Presentation.Controllers.Complaints.ViewModels
{
    public class GetComplaintApi
    {
        public long Id { get; set; }
        public String Title { get; private set; }
        public String Result { get; private set; }
        public DateTime CreateDate { get; private set; }
        public String Description { get; private set; }

        public GetComplainingApi Complaining { get; private set; }
        public GetPlaintiffApi Plaintiff { get; private set; }
        public IEnumerable<GetComplaintDocumentApi> Documents { get; private set; }


        public static GetComplaintApi Create(Complaint entity) =>
            new GetComplaintApi
            {
                Id = entity.Id,
                Title = entity.Title,
                Result = entity.Result.ToPersian(),
                CreateDate = entity.CreateDate,
                Description = entity.Description,
                Complaining = GetComplainingApi.Create(entity.Complaining),
                Plaintiff = GetPlaintiffApi.Create(entity.Plaintiff),
                Documents = GetComplaintDocumentApi.Create(entity.Documents)
            };
    }

    public class GetPlaintiffApi
    {
        public string FullName { get; private set; }
        public String PhoneNumber { get; private set; }
        public string Address { get; private set; }
        public String NationalCode { get; private set; }

        public static GetPlaintiffApi Create(Plaintiff entity) =>
            new GetPlaintiffApi
            {
                FullName = entity.FullName,
                Address = entity.Address,
                NationalCode = entity.NationalCode.Value,
                PhoneNumber = entity.PhoneNumber.Value
            };
    }

    public class GetComplainingApi
    {
        public string FullName { get; private set; }
        public String StudentNumber { get; private set; }
        public String NationalCode { get; private set; }
        public String? Grade { get; private set; }
        public String? EducationalGroup { get; private set; }
        public String? College { get; private set; }
        public String? Father { get; private set; }

        public static GetComplainingApi Create(Complaining entity) =>
            new GetComplainingApi
            {
                FullName = entity.FullName,
                Father = entity.Father,
                College = entity.College,
                EducationalGroup = entity.EducationalGroup,
                Grade = entity.Grade,
                NationalCode = entity.NationalCode.Value,
                StudentNumber = entity.StudentNumber.Value
            };
    }

    public class GetComplaintDocumentApi
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime SendTime { get; set; }

        public static IEnumerable<GetComplaintDocumentApi> Create(IEnumerable<ComplaintDocument> entities) =>
            entities.Select(entity=> new GetComplaintDocumentApi
            {
                Id = entity.Id,
                Name = entity.Name,
                SendTime = entity.SendTime
            });
    }
}
