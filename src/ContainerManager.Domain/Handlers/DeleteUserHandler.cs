using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Models;
using ContainerManager.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ContainerManager.Domain.Handlers
{
	public class DeleteUserHandler : IRequestHandler<DeleteCommand<User>, Unit>
	{
		private readonly IUserRepository _repo;
		private readonly IApplicationRepository _appRepo;
		private readonly IMachineRepository _machineRepo;

		public DeleteUserHandler(IUserRepository repo, IApplicationRepository appRepo, IMachineRepository machineRep)
		{
			_repo = repo;
			_appRepo = appRepo;
			_machineRepo = machineRep;
		}

		public async Task<Unit> Handle(DeleteCommand<User> request, CancellationToken cancellationToken)
		{
			//Change ownership of user apps to auth user
			await _appRepo.UpdateOwnership(request.Id, request.AuthUserId);

			//Change ownership of user machines to auth user
			await _machineRepo.UpdateOwnership(request.Id, request.AuthUserId);

			//Delete user
			await _repo.DeleteAsync(request.Id);
			return Unit.Value;
		}
	}
}
