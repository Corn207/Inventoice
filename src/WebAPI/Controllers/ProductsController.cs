using Application.Exceptions;
using Application.Services;
using Domain.DTOs;
using Domain.DTOs.Products;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController(ProductService service) : ControllerBase
{
	[HttpGet]
	public async Task<IEnumerable<ProductShort>> Get(
		[FromQuery] string? nameOrBarcode = null,
		[FromQuery] OrderBy orderBy = OrderBy.Ascending,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15)
	{
		var pagination = new Pagination(pageNumber, pageSize);

		return await service.SearchAsync(
			nameOrBarcode ?? string.Empty,
			orderBy,
			pagination);
	}

	[HttpGet("count")]
	public async Task<uint> Count(
		[FromQuery] string? nameOrBarcode = null,
		[FromQuery] OrderBy orderBy = OrderBy.Ascending)
	{
		return await service.CountAsync(
			nameOrBarcode ?? string.Empty,
			orderBy);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Product>> Get(string id)
	{
		var product = await service.GetAsync(id)
			?? throw new InvalidIdException("ProductId was not found.", id);

		return product;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] ProductCreateUpdate body)
	{
		var newId = await service.CreateAsync(body);
		return CreatedAtAction(nameof(Get), new { id = newId }, null);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Put(string id, [FromBody] ProductCreateUpdate body)
	{
		await service.ReplaceAsync(id, body);

		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		await service.DeleteAsync(id);

		return NoContent();
	}
}
