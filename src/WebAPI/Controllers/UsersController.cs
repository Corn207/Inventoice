using Application.Exceptions;
using Application.Services;
using Domain.DTOs;
using Domain.DTOs.Users;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

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

	[HttpGet("count")]
	public async Task<uint> Count(
		[FromQuery] string? name = null)
	{
		return await service.CountAsync(
			name ?? string.Empty);
	}

	[HttpGet("count/all")]
	public async Task<uint> CountAll()
	{
		return await service.CountAllAsync();
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<User>> Get(string id)
	{
		var entity = await service.GetAsync(id)
			?? throw new InvalidIdException("UserId was not found.", id);

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
		await service.ReplaceAsync(id, body);

		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		await service.DeleteAsync(id);

		return NoContent();
	}

	//[HttpGet("me")]
	//public async Task<User> GetMe()
	//{
	//	var entity = await service.GetAsync(User.Claims)
	//}
}
