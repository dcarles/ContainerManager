using ContainerManager.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerManager.Domain.Commands
{
	public class CreateMachineCommand : IRequest<Machine>
	{
		public CreateMachineCommand()
		{
			Id = Guid.NewGuid();
		}

		public string Name { get; set; }
		public OSType OS { get; set; }

		internal readonly Guid Id;

	}
}
