using Application.Exceptions;
using Application.Services;
using Domain.DTOs;
using Domain.DTOs.ImportReports;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ImportReportsController(ImportReportService service) : ControllerBase
{
	[HttpGet]
	public async Task<IEnumerable<ImportReportShort>> Get(
		[FromQuery] string? productNameOrBarcode = null,
		[FromQuery] string? authorName = null,
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
			authorName ?? string.Empty,
			timeRange,
			orderBy,
			pagination);
	}

	[HttpGet("count")]
	public async Task<uint> Count(
		[FromQuery] string? productNameOrBarcode = null,
		[FromQuery] string? authorName = null,
		[FromQuery] DateTime? dateStart = null,
		[FromQuery] DateTime? dateEnd = null,
		[FromQuery] OrderBy orderBy = OrderBy.Descending)
	{
		var timeRange = new TimeRange(dateStart ?? DateTime.MinValue, dateEnd ?? DateTime.MaxValue);

		return await service.CountAsync(
			productNameOrBarcode ?? string.Empty,
			authorName ?? string.Empty,
			timeRange,
			orderBy);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<ImportReport>> Get(string id)
	{
		var entity = await service.GetAsync(id)
			?? throw new InvalidIdException("ImportReportId was not found.", id);

		return entity;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] ImportReportCreate body)
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
}
