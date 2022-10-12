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
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContainerManager.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{

		private readonly ILogger<UserController> _logger;
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public UserController(ILogger<UserController> logger, IMediator mediator, IMapper mapper)
		{
			_logger = logger;
			_mediator = mediator;
			_mapper = mapper;
		}

		// GET api/User/5
		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			var userQuery = new GetByIdQuery<User>(id);
			var userResponse = await _mediator.Send(userQuery);
			if (userResponse != null)
				return Ok(userResponse);
			else
				return new NotFoundObjectResult(new ErrorResponse { StatusCode = 404, Message = "User requested does not Exists" });
		}


		// POST api/User
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
		{
			var userCommand = _mapper.Map<CreateUserCommand>(userRequest);
			try
			{
				var userResponse = await _mediator.Send(userCommand);
				var userUrl = $"{HttpContext.Request.GetEncodedUrl()}/{userResponse.Id}";
				return Created(userUrl, userResponse);
			}
			catch (RecordAlreadyExistsException ex)
			{
				return new ConflictObjectResult(new ErrorResponse { StatusCode = 409, Message = ex.Message });
			}
		}


		// DELETE api/User/5
		[Authorize(Policy = Policies.OnlyApiOwners)]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var deleteCommand = new DeleteCommand<User>(id);
			await _mediator.Send(deleteCommand);
			return Ok();
		}
	}
}
