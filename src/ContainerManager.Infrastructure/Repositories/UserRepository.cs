using AutoMapper;
using ContainerManager.Domain.Repositories;
using ContainerManager.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContainerManager.Infrastructure.Repositories
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		private readonly IMapper _mapper;

		public UserRepository(IMapper mapper) : base() => _mapper = mapper;


		public new async Task<Domain.Models.User> GetByIdAsync(Guid id)
		{
			return _mapper.Map<Domain.Models.User>(await base.GetByIdAsync(id));
		}

		public async Task<Domain.Models.User> GetByApiKey(string key)
		{
			return _mapper.Map<Domain.Models.User>(await base.GetSingleByQueryAsync(m => m.ApiKey == key));
		}

		public async Task<Domain.Models.User> GetByEmail(string email)
		{
			return _mapper.Map<Domain.Models.User>(await base.GetSingleByQueryAsync(m => m.Email == email));
		}

		public async Task AddAsync(Domain.Models.User user)
		{
			await base.AddAsync(_mapper.Map<User>(user));
		}

		public new async Task DeleteAsync(Guid id)
		{
			await base.DeleteAsync(id);
		}
	}
}
