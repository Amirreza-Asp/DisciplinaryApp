namespace DisciplinarySystem.Application.Authentication.Dtos
{
    public class LoginResultDto
    {
        private LoginResultDto () { }

        public bool Success { get; set; }
        public String Message { get; set; }

        public static LoginResultDto Successful () => new LoginResultDto { Success = true };
        public static LoginResultDto Faild ( String message ) => new LoginResultDto { Success = false , Message = message };
    }
}
