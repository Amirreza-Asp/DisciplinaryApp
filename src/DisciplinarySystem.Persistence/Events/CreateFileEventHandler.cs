using DisciplinarySystem.SharedKernel.Events;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace DisciplinarySystem.Persistence.Events
{
	public class CreateFileEventHandler : IRequestHandler<CreateFileEvent>
	{
		private readonly IHostingEnvironment _webHostEnv;

		public CreateFileEventHandler(IHostingEnvironment webHostEnv)
		{
			_webHostEnv = webHostEnv;
		}

		public async Task<Unit> Handle(CreateFileEvent request, CancellationToken cancellationToken)
		{
			var upload = _webHostEnv.WebRootPath;
			if (!Directory.Exists(upload + request.Path))
			{
				Directory.CreateDirectory(upload + request.Path);
			}


			string path = upload + request.Path + request.Document.Name;
			await File.WriteAllBytesAsync(path, request.Document.File);
			return Unit.Value;
		}
	}
}
