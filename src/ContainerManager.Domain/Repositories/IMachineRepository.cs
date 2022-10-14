using ContainerManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContainerManager.Domain.Repositories
{
	public interface IMachineRepository
	{
		Task AddAsync(Machine machine);
		Task<Machine> GetByIdAsync(Guid id);
		Task<IEnumerable<Machine>> GetByOwner(Guid userId);
		Task UpdateAsync(Machine machine);
		Task DeleteAsync(Guid id);
		Task UpdateOwnership(Guid currentOwnerId, Guid newOwnerId);
	}
}