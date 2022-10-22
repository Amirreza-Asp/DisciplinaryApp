using DisciplinarySystem.Application.Complaints.ViewModels.Create;
using DisciplinarySystem.Domain.Complaints;

namespace DisciplinarySystem.Application.Complaints.ViewModels.Update
{
    public class UpdatePlaintiff : CreatePlaintiff
    {
        public Guid Id { get; set; }

        public static UpdatePlaintiff CreateWith(Plaintiff plaintiff)
        {
            return new UpdatePlaintiff
            {
                Id = plaintiff.Id,
                Address = plaintiff.Address,
                FullName = plaintiff.FullName,
                NationalCode = plaintiff.NationalCode,
                PhoneNumber = plaintiff.PhoneNumber
            };
        }
    }
}
