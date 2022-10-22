using Ardalis.Specification;

namespace DisciplinarySystem.Domain.Users.Specification
{
	public class GetRoleByTitleSpec : Specification<Role>, ISingleResultSpecification
	{
		public GetRoleByTitleSpec(String title)
		{
			Query
				.Where(x => x.Title == title)
				.AsNoTracking();
		}
	}
}
