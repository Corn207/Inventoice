using Application.Exceptions;
using Application.Services;
using Domain.DTOs;
using Domain.DTOs.ExportReports;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ExportReportsController(ExportReportService service) : ControllerBase
{
	[HttpGet]
	public async Task<IEnumerable<ExportReportShort>> Get(
		[FromQuery] string? productNameOrBarcode = null,
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
			timeRange,
			orderBy,
			pagination);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<ExportReport>> Get(string id)
	{
		var entity = await service.GetAsync(id);
		if (entity is null)
		{
			return NotFound();
		}

		return entity;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] ExportReportCreate body)
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
			return BadRequest(ex.Ids);
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

		return NoContent();
	}
}
