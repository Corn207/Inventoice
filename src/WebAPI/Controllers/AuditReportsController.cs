using Application.Services;
using Domain.DTOs.AuditReports;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuditReportsController : ControllerBase
{
	private readonly AuditReportService _auditReportService;

	public AuditReportsController(AuditReportService auditReportService)
	{
		_auditReportService = auditReportService;
	}

	[HttpGet]
	public async Task<IEnumerable<AuditReportShort>> Get(
		[FromQuery] string? search = null,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15,
		[FromQuery] DateTime? startDate = null,
		[FromQuery] DateTime? endDate = null,
		[FromQuery] bool isDescending = false)
	{
		return await _auditReportService.SearchAsync(search, pageNumber, pageSize, startDate, endDate, isDescending);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<AuditReport>> Get(string id)
	{
		var entity = await _auditReportService.GetAsync(id);
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
			var newId = await _auditReportService.CreateAsync(body);
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
			await _auditReportService.DeleteAsync(id);
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
