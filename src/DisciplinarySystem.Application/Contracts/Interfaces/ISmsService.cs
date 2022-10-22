namespace DisciplinarySystem.Application.Contracts.Interfaces
{
    public interface ISmsService
    {
        Task<bool> Send ( String message , String phoneNumber );

    }
}
