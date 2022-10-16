using ContainerManager.Domain.Models;

namespace ContainerManager.Domain.Repositories
{
	public interface IApplicationRepository
	{
		Task AddAsync(Application app);
		Task<Application> GetByIdAsync(Guid id);
		Task<IEnumerable<Application>> GetByOwner(Guid userId);
		Task DeleteAsync(Guid id);
		Task UpdateAsync(Application app);
		Task UpdateOwnership(Guid currentOwnerId, Guid newOwnerId);
	}
}