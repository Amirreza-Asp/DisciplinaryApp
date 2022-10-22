namespace DisciplinarySystem.Domain.Authentication
{
    public class UserRole
    {
        public UserRole ( long userId , long roleId )
        {
            UserId = userId;
            RoleId = roleId;
        }

        private UserRole () { }


        public long UserId { get; private set; }
        public long RoleId { get; private set; }

        public AuthUser User { get; private set; }
        public AuthRole Role { get; private set; }
    }
}
