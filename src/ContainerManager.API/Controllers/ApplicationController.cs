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
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContainerManager.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApplicationController : ControllerBase
	{

		private readonly ILogger<ApplicationController> _logger;
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public ApplicationController(ILogger<ApplicationController> logger, IMediator mediator, IMapper mapper)
		{
			_logger = logger;
			_mediator = mediator;
			_mapper = mapper;

		}

		// GET api/application
		[HttpGet()]
		[Authorize]
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

		// GET api/application/5
		[HttpGet("{id}")]
		[Authorize]
		public async Task<IActionResult> Get(Guid id)
		{
			var applicationByIdQuery = new GetByIdQuery<Application>(id);
			var applicationResponse = await _mediator.Send(applicationByIdQuery);
			if (applicationResponse != null)
				return Ok(applicationResponse);
			else
				return new NotFoundObjectResult(new ErrorResponse { StatusCode = 404, Message = "Application requested does not Exists" });
		}

		// POST api/application
		[HttpPost]
		[Authorize]
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

		// PATCH api/application/{applicationId}
		[HttpPatch("{applicationId}")]
		[Authorize]
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
				return new ConflictObjectResult(new ErrorResponse { StatusCode = 409, Message = ex.Message });
			}

		}


		// DELETE api/application/5
		[HttpDelete("{id}")]
		[Authorize(Policy = Policies.OnlyApiOwners)]
		public async Task<IActionResult> Delete(Guid id)
		{
			var deleteCommand = new DeleteCommand<Application>(id);
			await _mediator.Send(deleteCommand);
			return Ok();
		}
	}
}
