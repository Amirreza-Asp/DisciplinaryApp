using DisciplinarySystem.Domain.Authentication;
using DisciplinarySystem.Domain.Authentication.Interfaces;
using DisciplinarySystem.Persistence.Data;

namespace DisciplinarySystem.Persistence.Repositories.Authentication
{
	public class UserRoleRepository : IUserRoleRepository
	{
		private readonly ApplicationDbContext _context;

		public UserRoleRepository ( ApplicationDbContext context )
		{
			_context = context;
		}

		public void Add ( UserRole userRole )
		{
			_context.UserRoles.Add(userRole);
		}

		public void Remove ( UserRole userRole )
		{
			_context.UserRoles.Remove(userRole);
		}

		public async Task SaveAsync ()
		{
			await _context.SaveChangesAsync();
		}
	}
}
