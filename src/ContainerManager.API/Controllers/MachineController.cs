using AutoMapper;
using ContainerManager.API.Auth;
using ContainerManager.API.ViewModels;
using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Exceptions;
using ContainerManager.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContainerManager.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MachineController : ControllerBase
	{
		private readonly ILogger<MachineController> _logger;
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public MachineController(ILogger<MachineController> logger, IMediator mediator, IMapper mapper)
		{
			_logger = logger;
			_mediator = mediator;
			_mapper = mapper;
		}


		// GET api/machine?ownerId=80927d68-ce2c-4c89-9805-de92d81b7517
		[HttpGet()]
		[Authorize]
		public async Task<IActionResult> Get()
		{
			var ownerId = Guid.Parse(User.Identity.Name);
			var machineByIdQuery = new GetByUserIdQuery<Machine>(ownerId);
			var machinesResponse = await _mediator.Send(machineByIdQuery);
			if (machinesResponse != null && machinesResponse.Any())
				return Ok(machinesResponse);
			else
				return new NotFoundObjectResult(new ErrorResponse { StatusCode = 404, Message = "The user does not own any machines or user does not exists" });
		}

		// GET api/machine/5
		[HttpGet("{id}")]
		[Authorize]
		public async Task<IActionResult> Get(Guid id)
		{
			var machineByIdQuery = new GetByIdQuery<Machine>(id);
			var machineResponse = await _mediator.Send(machineByIdQuery);
			if (machineResponse != null)
				return Ok(machineResponse);
			else
				return new NotFoundObjectResult(new ErrorResponse { StatusCode = 404, Message = "Machine requested does not Exists" });
		}

		// POST api/machine
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Post([FromBody] MachineRequest machineRequest)
		{
			var machineCommand = _mapper.Map<CreateMachineCommand>(machineRequest);
			machineCommand.OwnerId = Guid.Parse(User.Identity.Name);

			try
			{
				var machineResponse = await _mediator.Send(machineCommand);
				var machineUrl = $"{HttpContext.Request.GetEncodedUrl()}/{machineResponse.Id}";
				return Created(machineUrl, machineResponse);
			}
			catch (RecordAlreadyExistsException ex)
			{
				return new ConflictObjectResult(new ErrorResponse { StatusCode = 409, Message = ex.Message });
			}
		}


		// DELETE api/machine/5
		[HttpDelete("{id}")]
		[Authorize(Policy = Policies.OnlyApiOwners)]
		public async Task<IActionResult> Delete(Guid id)
		{
			var deleteCommand = new DeleteCommand<Machine>(id);
			await _mediator.Send(deleteCommand);
			return Ok();
		}
	}
}
