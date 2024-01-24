using Application.Exceptions;
using Application.Services;
using Domain.DTOs;
using Domain.DTOs.ImportReports;
using Domain.Entities;
using Identity.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = nameof(Role.Manager))]
public class ImportReportsController(ImportReportService service) : ControllerBase
{
	[HttpGet("search")]
	[ProducesResponseType<PartialArray<ImportReportShort>>(StatusCodes.Status200OK)]
	public async Task<PartialArray<ImportReportShort>> Get(
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
		var result = await service.SearchAsync(
			productNameOrBarcode,
			authorName,
			timeRange,
			orderBy,
			pagination);

		return result;
	}

	[HttpGet("total")]
	[ProducesResponseType<uint>(StatusCodes.Status200OK)]
	public async Task<uint> CountAll()
	{
		return await service.CountAllAsync();
	}

	[HttpGet("{id}")]
	[ProducesResponseType<ImportReport>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<ImportReport>> Get(string id)
	{
		var entity = await service.GetAsync(id)
			?? throw new NotFoundException("ImportReport's Id was not found.", id);

		return entity;
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Post([FromBody] ImportReportCreate body)
	{
		var userId = this.GetUserId();
		var newId = await service.CreateAsync(userId, body);

		return CreatedAtAction(nameof(Get), new { id = newId }, null);
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete(string id)
	{
		await service.DeleteAsync(id);

		return NoContent();
	}

	[HttpPatch("{id}/cancel")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Cancel(string id)
	{
		await service.CancelAsync(id);

		return NoContent();
	}
}
