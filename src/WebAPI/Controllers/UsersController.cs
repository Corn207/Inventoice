using Application.Exceptions;
using Application.Services;
using Domain.DTOs;
using Domain.DTOs.Users;
using Domain.Entities;
using Identity.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController(UserService service) : ControllerBase
{
	[HttpGet("search")]
	[Authorize(Roles = nameof(Role.Admin))]
	[ProducesResponseType<PartialArray<UserShort>>(StatusCodes.Status200OK)]
	public async Task<PartialArray<UserShort>> Search(
		[FromQuery] string? name = null,
		[FromQuery] OrderBy orderBy = OrderBy.Ascending,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15)
	{
		var pagination = new Pagination(pageNumber, pageSize);
		var result = await service.SearchAsync(
			name,
			orderBy,
			pagination);

		return result;
	}

	[HttpGet("total")]
	[Authorize(Roles = nameof(Role.Admin))]
	[ProducesResponseType<uint>(StatusCodes.Status200OK)]
	public async Task<uint> CountAll()
	{
		return await service.CountAllAsync();
	}

	[HttpGet("{id}")]
	[Authorize(Roles = nameof(Role.Admin))]
	[ProducesResponseType<User>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<User>> Get(string id)
	{
		var entity = await service.GetAsync(id)
			?? throw new NotFoundException("User's Id was not found.", id);

		return entity;
	}

	#region Me
	[HttpGet("me")]
	[Authorize]
	[ProducesResponseType<User>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<User>> GetMe()
	{
		var userId = this.GetUserId();
		var entity = await service.GetAsync(userId);
		if (entity == null)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "UserId was not found.");
		}

		return entity;
	}

	[HttpPut("me")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> EditMe([FromBody] UserCreateUpdate body)
	{
		var userId = this.GetUserId();
		try
		{
			await service.ReplaceAsync(userId, body);
		}
		catch (NotFoundException)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "UserId was not found.");
		}

		return NoContent();
	}
	#endregion
}
