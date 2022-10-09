﻿using ContainerManager.Domain.Models;
using ContainerManager.Domain.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ContainerManager.Domain.Commands
{
	public class GetUserByIdQuery : IRequest<User>
	{
		public GetUserByIdQuery(Guid id)
		{
			Id = id;		
		}

		public readonly Guid Id;	

	}
}
