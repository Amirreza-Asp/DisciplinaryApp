namespace DisciplinarySystem.SharedKernel.Common
{
    public interface IUserApi
    {
        Task<UserInfo> GetUserAsync(String nationalCode);
    }
}
