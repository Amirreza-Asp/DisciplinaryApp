namespace DisciplinarySystem.Domain.Authentication
{
    public class AuthRole : BaseEntity<long>
    {
        public AuthRole ( string title , string? description )
        {
            Title = Guard.Against.NullOrEmpty(title);
            Description = description;
        }

        private AuthRole () { }

        public String Title { get; private set; }
        public String? Description { get; private set; }

        public ICollection<AuthUser> Users { get; private set; }

        public String PersianTitle () => Title switch
        {
            "Managment" => "مدیریت",
            "Admin" => "ادمین",
            "User" => "کاربر",
            _ => throw new NotImplementedException()
        };
    }
}
