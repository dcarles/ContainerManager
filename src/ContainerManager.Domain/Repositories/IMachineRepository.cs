using ContainerManager.Domain.Models;

namespace ContainerManager.Domain.Repositories
{
	public interface IMachineRepository
	{
		Task AddAsync(Machine machine);
		Task<Machine> GetByIdAsync(Guid id);
		Task<IEnumerable<Machine>> GetByOwner(Guid userId);		
		Task DeleteAsync(Guid id);
		Task UpdateOwnership(Guid currentOwnerId, Guid newOwnerId);
	}
}