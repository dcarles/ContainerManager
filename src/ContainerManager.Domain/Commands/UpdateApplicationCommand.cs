using ContainerManager.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using static ContainerManager.Domain.Models.Application;

namespace ContainerManager.Domain.Commands
{
	public class UpdateApplicationCommand : IRequest<Application>
	{		
		public Guid MachineId { get; set; }	
		public Guid Id { get; set; }
		public Guid OwnerId { get; set; }


	}
}
