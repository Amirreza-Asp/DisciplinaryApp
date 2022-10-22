using DisciplinarySystem.SharedKernel.Events;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace DisciplinarySystem.Persistence.Events
{
	public class RemoveFileEventHandler : IRequestHandler<RemoveFileEvent>
	{
		private readonly IHostingEnvironment _webHostEnv;

		public RemoveFileEventHandler(IHostingEnvironment webHostEnv)
		{
			_webHostEnv = webHostEnv;
		}


		public Task<Unit> Handle(RemoveFileEvent request, CancellationToken cancellationToken)
		{
			string path = _webHostEnv.WebRootPath + request.Path + request.Name;
			File.Delete(path);
			return Unit.Task;
		}
	}
}
