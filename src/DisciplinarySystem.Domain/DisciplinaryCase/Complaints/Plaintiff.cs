using DisciplinarySystem.Domain.Invitations;

namespace DisciplinarySystem.Domain.Complaints
{
    public class Plaintiff : BaseEntity<Guid>
    {
        public Plaintiff(Guid id, string fullName, PhoneNumber phoneNumber, string address, NationalCode nationalCode)
        {
            Id = id;
            FullName = Guard.Against.NullOrEmpty(fullName);
            PhoneNumber = Guard.Against.NullOrEmpty(phoneNumber);
            Address = Guard.Against.NullOrEmpty(address);
            NationalCode = Guard.Against.NullOrEmpty(nationalCode);
        }

        private Plaintiff() { }

        public string FullName { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public string Address { get; private set; }
        public NationalCode NationalCode { get; private set; }

        public Complaint Complaint { get; private set; }
        public ICollection<Invitation> Invitations { get; private set; }

        public Plaintiff WithFullName(String fullName)
        {
            FullName = Guard.Against.NullOrEmpty(fullName);
            return this;
        }

        public Plaintiff WithPhoneNumber(PhoneNumber phonNumber)
        {
            PhoneNumber = phonNumber;
            return this;
        }

        public Plaintiff WithAddress(String address)
        {
            Address = Guard.Against.NullOrEmpty(address);
            return this;
        }

        public Plaintiff WithNationalCode(NationalCode nationalCode)
        {
            NationalCode = nationalCode;
            return this;
        }
    }
}
