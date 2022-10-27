using DisciplinarySystem.Domain.Authentication;

namespace DisciplinarySystem.Domain.Commonications
{
    public class SMS : BaseEntity<Guid>
    {
        public SMS ( PhoneNumber phoneNumber , string text , long userId )
        {
            Id = Guid.NewGuid();
            SendDate = DateTime.Now;
            IsDeleted = false;
            PhoneNumber = phoneNumber;
            Text = Guard.Against.NullOrEmpty(text);
            UserId = Guard.Against.NegativeOrZero(userId);
        }

        private SMS () { }

        public PhoneNumber PhoneNumber { get; private set; }
        public String Text { get; private set; }
        public DateTime SendDate { get; private set; }
        public long UserId { get; private set; }
        public bool IsDeleted { get; private set; }

        public void Delete () => IsDeleted = true;

        public AuthUser User { get; private set; }
    }
}
