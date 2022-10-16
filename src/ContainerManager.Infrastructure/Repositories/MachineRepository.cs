using AutoMapper;
using ContainerManager.Domain.Exceptions;
using ContainerManager.Domain.Repositories;
using ContainerManager.Infrastructure.Entities;

namespace ContainerManager.Infrastructure.Repositories
{
	public class MachineRepository : Repository<Machine>, IMachineRepository
	{
		private readonly IMapper _mapper;

		public MachineRepository(IMapper mapper, ContainerManagerDbContext dbContext) : base(dbContext) => _mapper = mapper;

		public new async Task<Domain.Models.Machine> GetByIdAsync(Guid id)
		{
			return _mapper.Map<Domain.Models.Machine>(await base.GetByIdAsync(id));
		}

		public async Task<IEnumerable<Domain.Models.Machine>> GetByOwner(Guid userId)
		{
			return _mapper.Map<IEnumerable<Domain.Models.Machine>>(await GetByQueryAsync(m => m.OwnerId == userId));
		}

		public async Task AddAsync(Domain.Models.Machine machine)
		{
			var existing = await GetByName(machine.Name);

			if (existing != null)
				throw new RecordAlreadyExistsException($"Machine with Name '{machine.Name}' already exists");

			await base.AddAsync(_mapper.Map<Machine>(machine));
		}

		public async Task UpdateOwnership(Guid currentOwnerId, Guid newOwnerId)
		{
			//Change ownership of user machines to newOwnerId
			var userMachines = await GetByQueryAsync(m => m.OwnerId == currentOwnerId);
			foreach (var machine in userMachines)
			{
				machine.OwnerId = newOwnerId;
				await base.UpdateAsync(machine);
			}
		}

		public async Task DeleteAsync(Guid id)
		{
			await base.DeleteAsync(new Machine { Id = id });
		}

		private async Task<Domain.Models.Machine> GetByName(string name)
		{
			return _mapper.Map<Domain.Models.Machine>(await GetSingleByQueryAsync(m => m.Name == name, avoidTracking: true));
		}

	}
}
