using ContainerManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContainerManager.Domain.Repositories
{
	public interface IApplicationRepository
	{
		Task AddAsync(Application app);
		Task<Application> GetByIdAsync(Guid id);
		Task<IEnumerable<Application>> GetByOwner(Guid userId);
		Task DeleteAsync(Guid id);
	}
}