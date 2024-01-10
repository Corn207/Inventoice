using Application.Exceptions;
using Application.Services;
using Domain.DTOs;
using Domain.DTOs.Users;
using Domain.Entities;
using Identity.Application.Exceptions;
using Identity.Application.Services;
using Identity.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController(
	UserService service,
	IdentityService identityService) : ControllerBase
{
	[HttpGet]
	[Authorize(Roles = nameof(Role.Admin))]
	[ProducesResponseType<IEnumerable<UserShort>>(StatusCodes.Status200OK)]
	public async Task<IEnumerable<UserShort>> Search(
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
	[Authorize(Roles = nameof(Role.Admin))]
	[ProducesResponseType<uint>(StatusCodes.Status200OK)]
	public async Task<uint> Count(
		[FromQuery] string? name = null)
	{
		return await service.CountAsync(
			name ?? string.Empty);
	}

	[HttpGet("count/all")]
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
		var entity = await service.GetAsync(id);
		if (entity == null)
		{
			return NotFound();
		};

		return entity;
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = nameof(Role.Admin))]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete(string id)
	{
		try
		{
			await service.DeleteAsync(id);
		}
		catch (InvalidIdException)
		{
			return NotFound();
		}
		try
		{
			await identityService.DeleteIdentityAsync(id);
		}
		catch (EntityNotFoundException)
		{
			return NotFound();
		}

		return NoContent();
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
		catch (InvalidIdException)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "UserId was not found.");
		}

		return NoContent();
	}
	#endregion
}
