using Application.Exceptions;
using Application.Services;
using Domain.DTOs.Users;
using Identity.Application.Exceptions;
using Identity.Application.Services;
using Identity.Domain.DTOs;
using Identity.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class IdentitiesController(
	IdentityService identityService,
	ClaimService claimService,
	TokenService tokenService,
	UserService userService) : ControllerBase
{
	#region Authentication
	[HttpPost("login")]
	[ProducesResponseType<TokenInfo>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<ActionResult<TokenInfo>> Login([FromBody] Login body)
	{
		try
		{
			var tokenInfo = await tokenService.CreateTokenAsync(body);
			return Ok(tokenInfo);
		}
		catch (InvalidLoginException)
		{
			return Unauthorized();
		}
	}

	[HttpPost("logout")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Logout()
	{
		var userId = this.GetUserId();
		await tokenService.DeleteTokenByUserIdAsync(userId);

		return NoContent();
	}

	[HttpPatch("change-password")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> ChangePassword([FromBody] ChangePassword body)
	{
		var userId = this.GetUserId();
		try
		{
			await identityService.ChangePasswordAsync(userId, body);
		}
		catch (EntityNotFoundException)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "UserId was not found.");
		}
		catch (InvalidPasswordException)
		{
			return BadRequest("Invalid password.");
		}

		return NoContent();
	}

	[HttpPatch("{id}/reset-password")]
	[Authorize(Roles = nameof(Role.Admin))]
	[ProducesResponseType<string>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<string>> ResetPassword(string id)
	{
		try
		{
			var password = await identityService.ResetPasswordAsync(id);
			return Ok(password);
		}
		catch (EntityNotFoundException)
		{
			return NotFound();
		}
	}
	#endregion

	#region Onboarding
	[HttpGet("onboarding")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<bool>> OnboardingAvailability()
	{
		var isExistAdmin = await claimService.IsExistsAdminAsync();
		return Ok(!isExistAdmin);
	}

	[HttpPost("onboarding")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status410Gone)]
	public async Task<IActionResult> Onboarding([FromBody] Onboarding body)
	{
		var isExistsAdmin = await claimService.IsExistsAdminAsync();
		if (isExistsAdmin)
		{
			return StatusCode(StatusCodes.Status410Gone);
		}
		var isExistsIdentity = await identityService.IsExistsIdentityAsync(body.Username);
		if (isExistsIdentity)
		{
			return BadRequest($"Identity existed with username: {body.Username}");
		}

		var createUser = new UserCreateUpdate()
		{
			Name = body.Name,
			Phonenumber = body.Phonenumber,
		};
		var userId = await userService.CreateAsync(createUser);
		var createIdentity = new CreateIdentity()
		{
			Name = body.Name,
			Phonenumber = body.Phonenumber,
			Username = body.Username,
			Roles = Role.Admin | Role.Manager | Role.Employee,
		};
		await identityService.CreateIdentityAsync(userId, createIdentity, body.Password);

		return CreatedAtAction(nameof(Login), null);
	}
	#endregion

	#region CRUD Identity
	[HttpGet("{id}")]
	[Authorize(Roles = nameof(Role.Admin))]
	[ProducesResponseType<IdentityDetails>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<IdentityDetails>> GetIdentity(string id)
	{
		try
		{
			var identity = await identityService.GetIdentityAsync(id);
			return Ok(identity);
		}
		catch (EntityNotFoundException)
		{
			return NotFound();
		}
		catch (DatabaseOperationErrorException)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	[HttpPost]
	[Authorize(Roles = nameof(Role.Admin))]
	[ProducesResponseType<string>(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateIdentity([FromBody] CreateIdentity body)
	{
		var isExistsIdentity = await identityService.IsExistsIdentityAsync(body.Username);
		if (isExistsIdentity)
		{
			return BadRequest($"Identity existed with username: {body.Username}");
		}

		var createUser = new UserCreateUpdate()
		{
			Name = body.Name,
			Phonenumber = body.Phonenumber,
		};
		var userId = await userService.CreateAsync(createUser);
		var password = await identityService.CreateIdentityAsync(userId, body);

		return CreatedAtAction(nameof(GetIdentity), new { Id = userId }, password);
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = nameof(Role.Admin))]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> DeleteIdentity(string id)
	{
		try
		{
			await userService.DeleteAsync(id);
		}
		catch (NotFoundException)
		{
			return NotFound();
		}

		try
		{
			await identityService.DeleteIdentityAsync(id);
		}
		catch (EntityNotFoundException)
		{
		}

		return NoContent();
	}
	#endregion
}
