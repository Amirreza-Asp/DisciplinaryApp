namespace DisciplinarySystem.Domain.Authentication.Interfaces
{
	public interface IUserRoleRepository
	{
		public void Add ( UserRole userRole );
		public void Remove ( UserRole userRole );
		public Task SaveAsync ();
	}
}
