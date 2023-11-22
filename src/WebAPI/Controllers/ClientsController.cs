using Application.Services;
using Domain.DTOs.Clients;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
	private readonly ClientService _clientService;

	public ClientsController(ClientService clientService)
	{
		_clientService = clientService;
	}

	[HttpGet]
	public async Task<IEnumerable<ClientShort>> Get(
		[FromQuery] string? search = null,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15,
		[FromQuery] bool isDescending = false)
	{
		return await _clientService.SearchAsync(search, pageNumber, pageSize, isDescending);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Client>> Get(string id)
	{
		var client = await _clientService.GetAsync(id);
		if (client is null)
		{
			return NotFound();
		}

		return client;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] ClientCreateUpdate body)
	{
		var newId = await _clientService.CreateAsync(body);
		return CreatedAtAction(nameof(Get), new { id = newId }, null);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Put(string id, [FromBody] ClientCreateUpdate body)
	{
		try
		{
			await _clientService.ReplaceAsync(id, body);
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
			await _clientService.DeleteAsync(id);
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}

		return NoContent();
	}
}
