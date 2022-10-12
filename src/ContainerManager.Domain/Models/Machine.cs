using System;
using System.Collections.Generic;

namespace ContainerManager.Domain.Models
{
	public class Machine : BaseModel
	{
		public string Name { get; set; }
		public OSType OS { get; set; }
		public Guid OwnerId { get; set; }	
		public List<Application>? Applications { get; set; }

	}

	public enum OSType
	{
		Linux,
		Windows
	}
}
