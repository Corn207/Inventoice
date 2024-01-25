using Application.Exceptions;
using Application.Services;
using Domain.DTOs;
using Domain.DTOs.Clients;
using Domain.Entities;
using Identity.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{nameof(Role.Manager)},{nameof(Role.Employee)}")]
public class ClientsController(ClientService service) : ControllerBase
{
	[HttpGet("search")]
	[ProducesResponseType<PartialArray<ClientShort>>(StatusCodes.Status200OK)]
	public async Task<PartialArray<ClientShort>> Search(
		[FromQuery] string? nameOrPhonenumber = null,
		[FromQuery] OrderBy orderBy = OrderBy.Ascending,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15)
	{
		var pagination = new Pagination(pageNumber, pageSize);
		var result = await service.SearchAsync(
			nameOrPhonenumber,
			orderBy,
			pagination);

		return result;
	}

	[HttpGet("total")]
	[ProducesResponseType<uint>(StatusCodes.Status200OK)]
	public async Task<uint> Total()
	{
		return await service.CountAllAsync();
	}

	[HttpGet("{id}")]
	[ProducesResponseType<Client>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<Client>> Get(string id)
	{
		var client = await service.GetAsync(id)
			?? throw new NotFoundException("Client's Id was not found.", id);

		return client;
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public async Task<IActionResult> Create([FromBody] ClientCreateUpdate body)
	{
		var newId = await service.CreateAsync(body);

		return CreatedAtAction(nameof(Get), new { id = newId }, null);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update(string id, [FromBody] ClientCreateUpdate body)
	{
		await service.ReplaceAsync(id, body);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete(string id)
	{
		await service.DeleteAsync(id);

		return NoContent();
	}
}
