namespace DisciplinarySystem.Application.Users.ViewModels.User
{
    public class UpdateUser : CreateUser
    {
        public Guid Id { get; set; }


        public static UpdateUser Create(Domain.Users.User entity)
        {
            return new UpdateUser
            {
                Id = entity.Id,
                FullName = entity.FullName,
                NationalCode = entity.NationalCode,
                StartDate = entity.AttendenceTime.From,
                EndDate = entity.AttendenceTime.To,
                RoleId = entity.RoleId
            };
        }
    }
}