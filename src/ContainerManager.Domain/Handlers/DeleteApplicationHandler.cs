using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Models;
using ContainerManager.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ContainerManager.Domain.Handlers
{
	public class DeleteApplicationHandler : IRequestHandler<DeleteCommand<Application>, Unit>
	{
		private readonly IApplicationRepository _repo;

		public DeleteApplicationHandler(IApplicationRepository repo)
		{
			_repo = repo;
		}

		public async Task<Unit> Handle(DeleteCommand<Application> request, CancellationToken cancellationToken)
		{
			await _repo.DeleteAsync(request.Id);
			return Unit.Value;
		}
	}
}
