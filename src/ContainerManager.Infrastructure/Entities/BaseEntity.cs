using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerManager.Infrastructure.Entities
{
	public abstract class BaseEntity
	{
		public Guid Id { get; set; }
	}
}
