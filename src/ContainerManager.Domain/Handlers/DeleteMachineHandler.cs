using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Models;
using ContainerManager.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ContainerManager.Domain.Handlers
{
	public class DeleteMachineHandler : IRequestHandler<DeleteCommand<Machine>, Unit>
	{
		private readonly IMachineRepository _repo;

		public DeleteMachineHandler(IMachineRepository repo)
		{
			_repo = repo;
		}

		public async Task<Unit> Handle(DeleteCommand<Machine> request, CancellationToken cancellationToken)
		{
			await _repo.DeleteAsync(request.Id);
			return Unit.Value;
		}
	}
}
