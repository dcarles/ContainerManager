using AutoMapper;
using ContainerManager.API.ViewModels;
using ContainerManager.Domain.Commands;
using ContainerManager.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

		// GET api/<UserController>/5
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

		// POST api/<UserController>
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] UserRequest userRequest)
		{
			var userCommand = _mapper.Map<CreateUserCommand>(userRequest);
			var userResponse = await _mediator.Send(userCommand);
			var userUrl = $"{HttpContext.Request.GetEncodedUrl()}/{userResponse.Id}";
			return Created(userUrl, userResponse);
		}


		// DELETE api/<UserController>/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var deleteCommand = new DeleteCommand<User>(id);
			await _mediator.Send(deleteCommand);
			return Ok();
		}
	}
}
