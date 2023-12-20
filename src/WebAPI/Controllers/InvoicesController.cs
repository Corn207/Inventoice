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
		[FromQuery] string? productNameOrBarcode = null,
		[FromQuery] string? clientNameOrPhonenumber = null,
		[FromQuery] string? authorName = null,
		[FromQuery] InvoiceStatus status = InvoiceStatus.All,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15,
		[FromQuery] DateTime? dateStart = null,
		[FromQuery] DateTime? dateEnd = null,
		[FromQuery] OrderBy orderBy = OrderBy.Descending)
	{
		var timeRange = new TimeRange(dateStart ?? DateTime.MinValue, dateEnd ?? DateTime.MaxValue);
		var pagination = new Pagination(pageNumber, pageSize);

		return await service.SearchAsync(
			productNameOrBarcode ?? string.Empty,
			clientNameOrPhonenumber ?? string.Empty,
			authorName ?? string.Empty,
			status,
			timeRange,
			orderBy,
			pagination);
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
			return NotFound(new { ex.Message, ex.Ids });
		}
		catch (OutOfStockException ex)
		{
			return BadRequest(ex.Message);
		}
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
		catch (UnknownException)
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
		catch (InvalidIdException ex)
		{
			return NotFound(new { ex.Message, ex.Ids });
		}
		catch (UnknownException)
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
		catch (InvalidIdException ex)
		{
			return NotFound(new { ex.Message, ex.Ids });
		}
		catch (UnknownException)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		return NoContent();
	}
}
