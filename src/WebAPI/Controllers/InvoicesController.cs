using Application.Exceptions;
using Application.Services;
using Domain.DTOs.Invoices;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class InvoicesController : ControllerBase
{
	private readonly InvoiceService _service;

	public InvoicesController(InvoiceService service)
	{
		_service = service;
	}

	[HttpGet]
	public async Task<IEnumerable<InvoiceShort>> Get(
		[FromQuery] string? product = null,
		[FromQuery] string? client = null,
		[FromQuery] string? author = null,
		[FromQuery] InvoiceStatus? status = null,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15,
		[FromQuery] DateTime? from = null,
		[FromQuery] DateTime? to = null,
		[FromQuery] bool isDescending = false)
	{
		return await _service.SearchAsync(
			product,
			client,
			author,
			status,
			pageNumber,
			pageSize,
			from,
			to,
			isDescending);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Invoice>> Get(string id)
	{
		var entity = await _service.GetAsync(id);
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
			var newId = await _service.CreateAsync(body);
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
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		return NoContent();
	}

	[HttpPatch("{id}/pay")]
	public async Task<IActionResult> Pay(string id, [FromBody] uint amount)
	{
		try
		{
			await _service.PayAsync(id, amount);
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
