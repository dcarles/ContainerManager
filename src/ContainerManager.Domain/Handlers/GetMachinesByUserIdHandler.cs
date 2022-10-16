using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Models;
using ContainerManager.Domain.Repositories;
using MediatR;

namespace ContainerManager.Domain.Handlers
{
	public class GetMachinesByUserIdHandler : IRequestHandler<GetByUserIdQuery<Machine>, IEnumerable<Machine>>
	{
		private readonly IMachineRepository _repo;

		public GetMachinesByUserIdHandler(IMachineRepository repo)
		{
			_repo = repo;
		}

		public async Task<IEnumerable<Machine>> Handle(GetByUserIdQuery<Machine> request, CancellationToken cancellationToken)
		{
			return await _repo.GetByOwner(request.Id);
		}
	}
}
