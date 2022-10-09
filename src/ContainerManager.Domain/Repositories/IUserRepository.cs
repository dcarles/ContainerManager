using System;
using System.Threading.Tasks;

namespace ContainerManager.Domain.Repositories
{
	public interface IUserRepository
	{
		Task AddAsync(Models.User user);
		Task<Models.User> GetByApiKey(string key);
		Task<Models.User> GetByEmail(string email);
		Task<Models.User> GetByIdAsync(Guid id);
		Task DeleteAsync(Guid id);
	}
}