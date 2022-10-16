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
	[ApiExplorerSettings(GroupName = "User")]
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

		/// <summary>
		/// Get existing user details
		/// </summary>
		/// <remarks>
		/// ## Description
		/// This endpoint will return an existing user. 
		/// If caller is ApiOwner it can retrieve any user, otherwise only caller user details can be retrieved
		/// </remarks>
		/// <param name="id">Id of the user to request</param>
		/// <returns>Existing user details matching the requested id</returns>
		[Authorize]
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(User), 200)]
		[ProducesResponseType(typeof(ErrorResponse), 404)]
		[ProducesResponseType(typeof(ErrorResponse), 500)]
		public async Task<IActionResult> Get(Guid id)
		{
			//If not Admin and try to get a different user than the one authenticated, then return forbidden error
			if (!User.IsInRole(UserRole.ApiOwner.ToString()) && id != Guid.Parse(User.Identity.Name))
			{
				return new ForbidResult();
			}

			var userQuery = new GetByIdQuery<User>(id);
			var userResponse = await _mediator.Send(userQuery);
			if (userResponse != null)
				return Ok(userResponse);
			else
				return new NotFoundObjectResult(new ErrorResponse { StatusCode = 404, Message = "User requested does not Exists" });
		}


		/// <summary>
		/// Register a new user
		/// </summary>
		/// <remarks>
		/// ## Description
		/// This endpoint will register a new user. This endpoint require no authentication
		/// The Email field is unique and the endpoint would return an 409 error response if is already in use
		/// </remarks>
		/// <param name="userRequest"></param>
		/// <returns>User created (including the ApiKey which you would need to authenticate for other endpoints)</returns>
		[AllowAnonymous]
		[HttpPost]
		[ProducesResponseType(typeof(User), 200)]
		[ProducesResponseType(typeof(ErrorResponse), 409)]
		[ProducesResponseType(typeof(ErrorResponse), 500)]
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


		/// <summary>
		/// Delete an existing User
		/// </summary>
		/// <remarks>
		/// ## Description
		/// This endpoint will delete the User. Restricted to ApiOwners.
		/// Any application/machines that the user owned will be transferred to the caller.
		/// </remarks>
		/// <param name="id">Id of the user to delete</param>	
		[Authorize(Policy = Policies.OnlyApiOwners)]
		[HttpDelete("{id}")]
		[ApiExplorerSettings(GroupName = "Api Owner Only")]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(ErrorResponse), 500)]
		public async Task<IActionResult> Delete(Guid id)
		{
			var deleteCommand = new DeleteCommand<User>(id, Guid.Parse(User.Identity.Name));
			await _mediator.Send(deleteCommand);
			return Ok();
		}
	}
}
