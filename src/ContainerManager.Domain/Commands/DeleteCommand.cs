﻿using MediatR;
using System;

namespace ContainerManager.Domain.Commands
{
	public class DeleteCommand<T> : IRequest<Unit>
	{
		public DeleteCommand(Guid id)
		{
			Id = id;
		}

		public readonly Guid Id;
	}
}
