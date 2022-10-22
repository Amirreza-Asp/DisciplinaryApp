using DisciplinarySystem.Domain.Users;

namespace DisciplinarySystem.Presentation.Controllers.Users.ViewModels
{
	public class GetAllUsers
	{
		public IEnumerable<User> Users { get; set; }
		public int TotalCount { get; set; }
		public UserFilter Filters { get; set; }
	}
}
