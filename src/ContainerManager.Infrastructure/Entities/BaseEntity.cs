using System;
using System.ComponentModel.DataAnnotations;

namespace ContainerManager.Infrastructure.Entities
{
	public abstract class BaseEntity
	{
		[Key]
		public Guid Id { get; set; }
	}
}
