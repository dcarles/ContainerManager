using MediatR;
using System;
using System.Collections.Generic;

namespace ContainerManager.Domain.Commands
{
	public class GetByUserIdQuery<T> : IRequest<IEnumerable<T>>
	{
		public GetByUserIdQuery(Guid id)
		{
			Id = id;
		}

		public readonly Guid Id;

	}
}
