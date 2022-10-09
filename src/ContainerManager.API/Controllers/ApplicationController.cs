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

		// GET api/<UserController>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			var applicationByIdQuery = new GetApplicationByIdQuery(id);
			var applicationResponse = await _mediator.Send(applicationByIdQuery);
			return Ok(applicationResponse);
		}

		// POST api/<UserController>
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] ApplicationRequest appRequest)
		{
			//appRequest.OwnerId = Guid.Parse(User.Identity.Name);
			var applicationCommand = _mapper.Map<CreateApplicationCommand>(appRequest);
			var applicationResponse = await _mediator.Send(applicationCommand);
			var applicationUrl = $"{HttpContext.Request.GetEncodedUrl()}/{applicationResponse.Id}";
			return Created(applicationUrl, applicationResponse);
		}
		

		// DELETE api/<UserController>/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			return Ok();
		}
	}
}
