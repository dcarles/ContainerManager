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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContainerManager.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[ApiExplorerSettings(GroupName = "Machine")]
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


		/// <summary>
		/// Get machines owned by caller
		/// </summary>
		/// <remarks>
		/// ## Description
		/// This endpoint will return all machines owned by the caller 		
		/// </remarks>
		/// <returns>A list of machines owned by the caller</returns>
		[HttpGet()]
		[Authorize]
		[ProducesResponseType(typeof(IEnumerable<Machine>), 200)]
		[ProducesResponseType(typeof(ErrorResponse), 404)]
		[ProducesResponseType(typeof(ErrorResponse), 500)]
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

		/// <summary>
		/// Create a new machine definition
		/// </summary>
		/// <remarks>
		/// ## Description
		/// This endpoint will create a new machine definition. 
		/// The machine will be owned by the caller
		/// </remarks>
		/// <param name="machineRequest">The machine definion to create</param>
		/// <returns>Machine created details</returns>
		[HttpPost]
		[Authorize]
		[ProducesResponseType(typeof(Machine), 200)]
		[ProducesResponseType(typeof(ErrorResponse), 409)]
		[ProducesResponseType(typeof(ErrorResponse), 500)]
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


		/// <summary>
		/// Delete an existing Machine
		/// </summary>
		/// <remarks>
		/// ## Description
		/// This endpoint will delete the Machine. Restricted to ApiOwners.
		/// Any application related to this machine will be updated to not be related to any machine.
		/// </remarks>
		/// <param name="id">Id of the machine to delete</param>	
		[HttpDelete("{id}")]
		[Authorize(Policy = Policies.OnlyApiOwners)]
		[ApiExplorerSettings(GroupName = "Api Owner Only")]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(ErrorResponse), 500)]
		public async Task<IActionResult> Delete(Guid id)
		{
			var deleteCommand = new DeleteCommand<Machine>(id, Guid.Parse(User.Identity.Name));
			await _mediator.Send(deleteCommand);
			return Ok();
		}
	}
}
