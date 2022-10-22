using DisciplinarySystem.Application.Authentication.Dtos;

namespace DisciplinarySystem.Application.Authentication.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResultDto> LoginAsync ( LoginDto command );

        Task ChangePasswordAsync ( ChangePasswordDto command );
    }
}
