using AutoMapper;
using ContainerManager.Domain.Exceptions;
using ContainerManager.Domain.Repositories;
using ContainerManager.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContainerManager.Infrastructure.Repositories
{
	public class ApplicationRepository : Repository<Application>, IApplicationRepository
	{
		private readonly IMapper _mapper;

		public ApplicationRepository(IMapper mapper, ContainerManagerDbContext dbContext) : base(dbContext) => _mapper = mapper;


		public new async Task<Domain.Models.Application> GetByIdAsync(Guid id)
		{
			return _mapper.Map<Domain.Models.Application>(await base.GetByIdAsync(id));
		}

		public async Task<IEnumerable<Domain.Models.Application>> GetByOwner(Guid userId)
		{
			return _mapper.Map<IEnumerable<Domain.Models.Application>>(await GetByQueryAsync(m => m.OwnerId == userId));
		}

		public async Task<Domain.Models.Application> GetByName(string name)
		{
			return _mapper.Map<Domain.Models.Application>(await GetSingleByQueryAsync(m => m.Name == name));
		}

		public async Task AddAsync(Domain.Models.Application app)
		{
			var existing = await GetByName(app.Name);

			if (existing != null)
				throw new RecordAlreadyExistsException($"Application with Name '{app.Name}' already exists");

			await base.AddAsync(_mapper.Map<Application>(app));
		}

		public async Task UpdateAsync(Domain.Models.Application app)
		{
			var existing = await GetByIdNoTrackingAsync(app.Id);

			if (existing == null || (existing != null && existing.OwnerId != app.OwnerId))
				throw new RecordNotFoundException($"Application with Id '{app.Id}' does not exists or you do not own it");

			existing.MachineId = app.MachineId;

			await base.UpdateAsync(_mapper.Map<Application>(existing));
		}

		public async Task DeleteAsync(Guid id)
		{
			await base.DeleteAsync(new Application { Id = id });
		}
	}
}
