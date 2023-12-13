using Application.Services;
using Domain.DTOs;
using Domain.DTOs.AuditReports;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuditReportsController(AuditReportService auditReportService) : ControllerBase
{
	[HttpGet]
	public async Task<IEnumerable<AuditReportShort>> Get(
		[FromQuery] string? search = null,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15,
		[FromQuery] DateTime? startDate = null,
		[FromQuery] DateTime? endDate = null,
		[FromQuery] OrderBy orderBy = OrderBy.Descending)
	{
		return await auditReportService.SearchAsync(
			search ?? string.Empty,
			pageNumber,
			pageSize,
			startDate ?? DateTime.MinValue,
			endDate ?? DateTime.MaxValue,
			orderBy);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<AuditReport>> Get(string id)
	{
		var entity = await auditReportService.GetAsync(id);
		if (entity is null)
		{
			return NotFound();
		}

		return entity;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] AuditReportCreate body)
	{
		try
		{
			var newId = await auditReportService.CreateAsync(body);
			return CreatedAtAction(nameof(Get), new { id = newId }, null);
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		try
		{
			await auditReportService.DeleteAsync(id);
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		return NoContent();
	}
}
