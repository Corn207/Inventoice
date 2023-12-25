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
		[FromQuery] DateTime? dateStart = null,
		[FromQuery] DateTime? dateEnd = null,
		[FromQuery] OrderBy orderBy = OrderBy.Descending,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15)
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


	[HttpGet("count")]
	public async Task<uint> Count(
		[FromQuery] string? productNameOrBarcode = null,
		[FromQuery] string? clientNameOrPhonenumber = null,
		[FromQuery] string? authorName = null,
		[FromQuery] InvoiceStatus status = InvoiceStatus.All,
		[FromQuery] DateTime? dateStart = null,
		[FromQuery] DateTime? dateEnd = null)
	{
		var timeRange = new TimeRange(dateStart ?? DateTime.MinValue, dateEnd ?? DateTime.MaxValue);

		return await service.CountAsync(
			productNameOrBarcode ?? string.Empty,
			clientNameOrPhonenumber ?? string.Empty,
			authorName ?? string.Empty,
			status,
			timeRange);
	}

	[HttpGet("count/all")]
	public async Task<uint> CountAll()
	{
		return await service.CountAllAsync();
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Invoice>> Get(string id)
	{
		var entity = await service.GetAsync(id)
			?? throw new InvalidIdException("InvoiceId was not found.", id);

		return entity;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] InvoiceCreate body)
	{
		var newId = await service.CreateAsync(body);
		return CreatedAtAction(nameof(Get), new { id = newId }, null);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		await service.DeleteAsync(id);

		return NoContent();
	}

	[HttpPatch("{id}/cancel")]
	public async Task<IActionResult> Cancel(string id)
	{
		await service.CancelAsync(id);

		return NoContent();
	}

	[HttpPatch("{id}/pay")]
	public async Task<IActionResult> Pay(string id)
	{
		await service.PayAsync(id);

		return NoContent();
	}
}
