using Domain.DTOs.Clients;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Domain.DTOs.Users;
using Domain.Entities;
using Application.Exceptions;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController(UserService service) : ControllerBase
{
	[HttpGet]
	public async Task<IEnumerable<UserShort>> Get(
		[FromQuery] string? name = null,
		[FromQuery] OrderBy orderBy = OrderBy.Ascending,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15)
	{
		var pagination = new Pagination(pageNumber, pageSize);

		return await service.SearchAsync(
			name ?? string.Empty,
			orderBy,
			pagination);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<User>> Get(string id)
	{
		var entity = await service.GetAsync(id);
		if (entity is null)
		{
			return NotFound();
		}

		return entity;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] UserCreate body)
	{
		var newId = await service.CreateAsync(body);
		return CreatedAtAction(nameof(Get), new { id = newId }, null);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Put(string id, [FromBody] UserUpdate body)
	{
		try
		{
			await service.ReplaceAsync(id, body);
		}
		catch (InvalidIdException ex)
		{
			return NotFound(new { ex.Message, ex.Ids });
		}

		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		try
		{
			await service.DeleteAsync(id);
		}
		catch (InvalidIdException ex)
		{
			return NotFound(new { ex.Message, ex.Ids });
		}

		return NoContent();
	}
}
