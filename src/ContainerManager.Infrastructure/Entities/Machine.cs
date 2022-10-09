﻿using ContainerManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerManager.Infrastructure.Entities
{
	public class Machine : BaseEntity
	{
		public string Name { get; set; }
		public OSType OS { get; set; }
		public Guid OwnerId { get; set; }
		public List<Application> Applications { get; set; }
	}
}
