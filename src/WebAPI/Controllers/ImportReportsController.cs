using Application.Exceptions;
using Application.Services;
using Domain.DTOs.ImportReports;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ImportReportsController : ControllerBase
{
	private readonly ImportReportService _service;

	public ImportReportsController(ImportReportService service)
	{
		_service = service;
	}

	[HttpGet]
	public async Task<IEnumerable<ImportReportShort>> Get(
		[FromQuery] string? search = null,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15,
		[FromQuery] DateTime? startDate = null,
		[FromQuery] DateTime? endDate = null,
		[FromQuery] bool isDescending = false)
	{
		return await _service.SearchAsync(search, pageNumber, pageSize, startDate, endDate, isDescending);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<ImportReport>> Get(string id)
	{
		var entity = await _service.GetAsync(id);
		if (entity is null)
		{
			return NotFound();
		}

		return entity;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] ImportReportCreate body)
	{
		try
		{
			var newId = await _service.CreateAsync(body);
			return CreatedAtAction(nameof(Get), new { id = newId }, null);
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
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
			await _service.DeleteAsync(id);
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
			await _service.CancelAsync(id);
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
		catch (OutOfStockException ex)
		{
			return BadRequest(ex.Ids);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		return NoContent();
	}
}
