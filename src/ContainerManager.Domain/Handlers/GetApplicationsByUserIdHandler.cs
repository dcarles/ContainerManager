using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Models;
using ContainerManager.Domain.Repositories;
using MediatR;

namespace ContainerManager.Domain.Handlers
{
	public class GetApplicationsByUserIdHandler : IRequestHandler<GetByUserIdQuery<Application>, IEnumerable<Application>>
	{
		private readonly IApplicationRepository _repo;

		public GetApplicationsByUserIdHandler(IApplicationRepository repo)
		{
			_repo = repo;
		}

		public async Task<IEnumerable<Application>> Handle(GetByUserIdQuery<Application> request, CancellationToken cancellationToken)
		{
			return await _repo.GetByOwner(request.Id);
		}
	}
}
