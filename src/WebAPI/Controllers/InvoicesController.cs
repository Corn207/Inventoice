using Application.Exceptions;
using Application.Services;
using Domain.DTOs;
using Domain.DTOs.Invoices;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class InvoicesController(InvoiceService service) : ControllerBase
{
	[HttpGet]
	public async Task<IEnumerable<InvoiceShort>> Get(
		[FromQuery] string? product = null,
		[FromQuery] string? client = null,
		[FromQuery] string? author = null,
		[FromQuery] InvoiceStatus status = InvoiceStatus.All,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15,
		[FromQuery] DateTime? startDate = null,
		[FromQuery] DateTime? endDate = null,
		[FromQuery] OrderBy orderBy = OrderBy.Descending)
	{
		return await service.SearchAsync(
			product,
			client,
			author,
			status,
			pageNumber,
			pageSize,
			startDate ?? DateTime.MinValue,
			endDate ?? DateTime.MaxValue,
			orderBy);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Invoice>> Get(string id)
	{
		var entity = await service.GetAsync(id);
		if (entity is null)
		{
			return NotFound();
		}

		return entity;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] InvoiceCreate body)
	{
		try
		{
			var newId = await service.CreateAsync(body);
			return CreatedAtAction(nameof(Get), new { id = newId }, null);
		}
		catch (InvalidIdException ex)
		{
			return BadRequest(ex.Message);
		}
		catch (OutOfStockException ex)
		{
			return BadRequest(ex.Message);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		try
		{
			await service.DeleteAsync(id);
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		return NoContent();
	}

	[HttpPatch("{id}/cancel")]
	public async Task<IActionResult> Cancel(string id)
	{
		try
		{
			await service.CancelAsync(id);
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		return NoContent();
	}

	[HttpPatch("{id}/pay")]
	public async Task<IActionResult> Pay(string id)
	{
		try
		{
			await service.PayAsync(id);
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		return NoContent();
	}
}
