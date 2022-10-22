using System.Linq.Expressions;

namespace DisciplinarySystem.Domain.DisciplinaryCase.Cases.Interfaces
{
	public interface ICaseReposiotry : IRepository<Case>
	{
		bool IsExist(long id);
		bool IsExist(Expression<Func<Case, bool>> filters);
		void UpdateStatus(long caseId, CaseStatus status);
	}
}
