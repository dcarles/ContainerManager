using AutoMapper;
using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Models;
using ContainerManager.Domain.Repositories;
using MediatR;

namespace ContainerManager.Domain.Handlers
{
	public class CreateUserHandler : IRequestHandler<CreateUserCommand, User>
	{
		private readonly IUserRepository _repo;
		private readonly IMapper _mapper;

		public CreateUserHandler(IUserRepository repo, IMapper mapper)
		{
			_repo = repo;
			_mapper = mapper;
		}

		public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
		{
			var user = _mapper.Map<User>(request);
			await _repo.AddAsync(user);
			return user;
		}
	}
}
