using Application.Services;
using Domain.DTOs;
using Domain.DTOs.Clients;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ClientsController(ClientService clientService) : ControllerBase
{
	[HttpGet]
	public async Task<IEnumerable<ClientShort>> Get(
		[FromQuery] string? search = null,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15,
		[FromQuery] OrderBy orderBy = OrderBy.Ascending)
	{
		return await clientService.SearchAsync(
			search ?? string.Empty,
			pageNumber,
			pageSize,
			orderBy);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Client>> Get(string id)
	{
		var client = await clientService.GetAsync(id);
		if (client is null)
		{
			return NotFound();
		}

		return client;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] ClientCreateUpdate body)
	{
		var newId = await clientService.CreateAsync(body);
		return CreatedAtAction(nameof(Get), new { id = newId }, null);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Put(string id, [FromBody] ClientCreateUpdate body)
	{
		try
		{
			await clientService.ReplaceAsync(id, body);
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		try
		{
			await clientService.DeleteAsync(id);
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}

		return NoContent();
	}
}
