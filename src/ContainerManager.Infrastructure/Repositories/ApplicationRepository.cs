﻿using AutoMapper;
using ContainerManager.Domain.Repositories;
using ContainerManager.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContainerManager.Infrastructure.Repositories
{
	public class ApplicationRepository : Repository<Application>, IApplicationRepository
	{
		private readonly IMapper _mapper;

		public ApplicationRepository(IMapper mapper) : base() => _mapper = mapper;


		public new async Task<Domain.Models.Application> GetByIdAsync(Guid id)
		{
			return _mapper.Map<Domain.Models.Application>(await base.GetByIdAsync(id));
		}

		public async Task<IEnumerable<Domain.Models.Application>> GetByOwner(Guid userId)
		{
			return _mapper.Map<IEnumerable<Domain.Models.Application>>(await GetByQueryAsync(m => m.OwnerId == userId));
		}

		public async Task AddAsync(Domain.Models.Application app)
		{
			await base.AddAsync(_mapper.Map<Application>(app));
		}

		public new async Task DeleteAsync(Guid id)
		{
			await base.DeleteAsync(id);
		}
	}
}