using ContainerManager.Domain.Models;
using System;
using System.Collections.Generic;

namespace ContainerManager.Infrastructure.Entities
{
	public class Machine : BaseEntity
	{
		public string Name { get; set; }
		public OSType OS { get; set; }
		public Guid OwnerId { get; set; }			
	}
}
