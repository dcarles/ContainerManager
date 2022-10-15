using AutoMapper;
using ContainerManager.API.Auth;
using ContainerManager.API.ViewModels;
using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Exceptions;
using ContainerManager.Domain.Models;
using FluentValidation.AspNetCore;
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
	/// <summary>
	/// Controler for Application CRUD operations
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	[ApiExplorerSettings(GroupName = "Application")]
	public class ApplicationController : ControllerBase
	{
		private readonly ILogger<ApplicationController> _logger;
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		/// <summary>
		/// Application Controler constructor
		/// </summary>
		/// <param name="logger">Logger to log errors and info</param>
		/// <param name="mediator">Mediator to send query/commands</param>
		/// <param name="mapper">Mapper to map request to command/query</param>
		public ApplicationController(ILogger<ApplicationController> logger, IMediator mediator, IMapper mapper)
		{
			_logger = logger;
			_mediator = mediator;
			_mapper = mapper;

		}

		/// <summary>
		/// Get applications owned by caller
		/// </summary>
		/// <remarks>
		/// ## Description
		/// This endpoint will return all applications owned by the caller 		
		/// </remarks>
		/// <returns>A list of applications owned by the caller</returns>
		[HttpGet()]
		[Authorize]
		[ProducesResponseType(typeof(IEnumerable<Application>), 200)]
		[ProducesResponseType(typeof(ErrorResponse), 404)]
		[ProducesResponseType(typeof(ErrorResponse), 500)]
		public async Task<IActionResult> Get()
		{
			var ownerId = Guid.Parse(User.Identity.Name);
			var applicationByUserIdQuery = new GetByUserIdQuery<Application>(ownerId);
			var applicationsResponse = await _mediator.Send(applicationByUserIdQuery);
			if (applicationsResponse != null && applicationsResponse.Any())
				return Ok(applicationsResponse);
			else
				return new NotFoundObjectResult(new ErrorResponse { StatusCode = 404, Message = "The user does not own any applications or user does not exists" });
		}

		/// <summary>
		/// Create a new application definition
		/// </summary>
		/// <remarks>
		/// ## Description
		/// This endpoint will create a new application definition. 
		/// The application will be owned by the caller and will not be associated to any machine. 
		/// Initial application state will be "created"
		/// </remarks>
		/// <param name="applicationRequest">The application definion to create</param>
		/// <returns>Application created details</returns>
		[HttpPost]
		[Authorize]
		[ProducesResponseType(typeof(Application), 200)]
		[ProducesResponseType(typeof(ErrorResponse), 409)]
		[ProducesResponseType(typeof(ErrorResponse), 500)]
		public async Task<IActionResult> Post([FromBody, CustomizeValidator(RuleSet = "Post")] ApplicationRequest appRequest)
		{
			var applicationCommand = _mapper.Map<CreateApplicationCommand>(appRequest);
			applicationCommand.OwnerId = Guid.Parse(User.Identity.Name);
			try
			{
				var applicationResponse = await _mediator.Send(applicationCommand);
				var applicationUrl = $"{HttpContext.Request.GetEncodedUrl()}/{applicationResponse.Id}";
				return Created(applicationUrl, applicationResponse);
			}
			catch (RecordAlreadyExistsException ex)
			{
				return new ConflictObjectResult(new ErrorResponse { StatusCode = 409, Message = ex.Message });
			}

		}

		/// <summary>
		/// Assign Application to a Machine
		/// </summary>
		/// <remarks>
		/// ## Description
		/// This endpoint will assign an application to a machine.	
		/// </remarks>
		/// <param name="applicationId">The id of the application to update</param>
		/// <param name="appRequest">The details of the fields to change (MachineId)</param>
		/// <returns>The updated application details</returns>
		[HttpPatch("{applicationId}")]
		[Authorize]
		[ProducesResponseType(typeof(Application), 200)]
		[ProducesResponseType(typeof(ErrorResponse), 404)]
		[ProducesResponseType(typeof(ErrorResponse), 500)]
		public async Task<IActionResult> Patch([FromRoute] Guid applicationId,
			[FromBody, CustomizeValidator(RuleSet = "Patch")] ApplicationRequest appRequest)
		{
			var applicationCommand = _mapper.Map<UpdateApplicationCommand>(appRequest);		
			applicationCommand.OwnerId = Guid.Parse(User.Identity.Name);
			applicationCommand.Id = applicationId;

			try
			{
				var applicationResponse = await _mediator.Send(applicationCommand);			
				return Ok(applicationResponse);
			}
			catch (RecordNotFoundException ex)
			{
				return new NotFoundObjectResult(new ErrorResponse { StatusCode = 404, Message = ex.Message });
			}
		}

		/// <summary>
		/// Delete an existing Application
		/// </summary>
		/// <remarks>
		/// ## Description
		/// This endpoint will delete the Application. Restricted to ApiOwners.	
		/// </remarks>
		/// <param name="id">Id of the application to delete</param>	
		[HttpDelete("{id}")]
		[Authorize(Policy = Policies.OnlyApiOwners)]
		[ApiExplorerSettings(GroupName = "Api Owner Only")]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(ErrorResponse), 500)]		
		public async Task<IActionResult> Delete(Guid id)
		{
			var deleteCommand = new DeleteCommand<Application>(id, Guid.Parse(User.Identity.Name));
			await _mediator.Send(deleteCommand);
			return Ok();
		}
	}
}
