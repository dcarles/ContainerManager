using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Models;
using ContainerManager.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ContainerManager.Domain.Handlers
{
	public class GetUserByIdHandler : IRequestHandler<GetByIdQuery<User>, User>
	{
		private readonly IUserRepository _repo;

		public GetUserByIdHandler(IUserRepository repo)
		{
			_repo = repo;
		}

		public async Task<User> Handle(GetByIdQuery<User> request, CancellationToken cancellationToken)
		{
			return await _repo.GetByIdAsync(request.Id);
		}
	}
}
