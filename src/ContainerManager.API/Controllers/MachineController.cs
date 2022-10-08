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

		// GET api/<UserController>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			return Ok(new Machine());
		}

		// POST api/<UserController>
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] MachineRequest machineRequest)
		{
			var machineCommand = _mapper.Map<CreateMachineCommand>(machineRequest);
			var machineResponse = await _mediator.Send(machineCommand);
			var machineUrl = $"{HttpContext.Request.GetEncodedUrl()}/{machineResponse.Id}";
			return Created(machineUrl, machineResponse);
		}
		

		// DELETE api/<UserController>/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			return Ok();
		}
	}
}
