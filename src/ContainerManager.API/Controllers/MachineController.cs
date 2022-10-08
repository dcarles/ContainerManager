using ContainerManager.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContainerManager.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MachineController : ControllerBase
	{		

		// GET api/<UserController>/5
		[HttpGet("{id}")]
		public Machine Get(Guid id)
		{
			return new Machine();
		}

		// POST api/<UserController>
		[HttpPost]
		public void Post([FromBody] Machine machine)
		{
		}
		

		// DELETE api/<UserController>/5
		[HttpDelete("{id}")]
		public void Delete(Guid id)
		{
		}
	}
}
