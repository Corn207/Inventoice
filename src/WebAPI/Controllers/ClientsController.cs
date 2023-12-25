using Application.Exceptions;
using Application.Services;
using Domain.DTOs;
using Domain.DTOs.Clients;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ClientsController(ClientService service) : ControllerBase
{
	[HttpGet]
	public async Task<IEnumerable<ClientShort>> Get(
		[FromQuery] string? nameOrPhonenumber = null,
		[FromQuery] OrderBy orderBy = OrderBy.Ascending,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15)
	{
		var pagination = new Pagination(pageNumber, pageSize);

		return await service.SearchAsync(
			nameOrPhonenumber ?? string.Empty,
			orderBy,
			pagination);
	}

	[HttpGet("count")]
	public async Task<uint> Count(
		[FromQuery] string? nameOrPhonenumber = null)
	{
		return await service.CountAsync(
			nameOrPhonenumber ?? string.Empty);
	}

	[HttpGet("count/all")]
	public async Task<uint> CountAll()
	{
		return await service.CountAllAsync();
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Client>> Get(string id)
	{
		var client = await service.GetAsync(id)
			?? throw new InvalidIdException("ClientId was not found.", id);

		return client;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] ClientCreateUpdate body)
	{
		var newId = await service.CreateAsync(body);

		return CreatedAtAction(nameof(Get), new { id = newId }, null);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Put(string id, [FromBody] ClientCreateUpdate body)
	{
		await service.ReplaceAsync(id, body);

		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		await service.DeleteAsync(id);

		return NoContent();
	}
}
