using DisciplinarySystem.Domain.Users;

namespace DisciplinarySystem.Presentation.Controllers.Users.ViewModels.Roles
{
	public class GetAllRoles
	{
		public List<Role> Roles { get; set; }
		public int TotalCount { get; set; }
		public RoleFilter Filters { get; set; }
	}
}
