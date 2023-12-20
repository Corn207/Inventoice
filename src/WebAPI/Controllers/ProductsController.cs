using Application.Exceptions;
using Application.Services;
using Domain.DTOs;
using Domain.DTOs.Products;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController(ProductService productService) : ControllerBase
{
	[HttpGet]
	public async Task<IEnumerable<ProductShort>> Get(
		[FromQuery] string? nameOrBarcode = null,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15,
		[FromQuery] OrderBy orderBy = OrderBy.Ascending)
	{
		var pagination = new Pagination(pageNumber, pageSize);

		return await productService.SearchAsync(
			nameOrBarcode ?? string.Empty,
			orderBy,
			pagination);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Product>> Get(string id)
	{
		var product = await productService.GetAsync(id);
		if (product is null)
		{
			return NotFound();
		}

		return product;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] ProductCreateUpdate body)
	{
		var newId = await productService.CreateAsync(body);
		return CreatedAtAction(nameof(Get), new { id = newId }, null);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Put(string id, [FromBody] ProductCreateUpdate body)
	{
		try
		{
			await productService.ReplaceAsync(id, body);
		}
		catch (InvalidIdException ex)
		{
			return NotFound(new { ex.Message, ex.Ids });
		}

		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		try
		{
			await productService.DeleteAsync(id);
		}
		catch (InvalidIdException ex)
		{
			return NotFound(new { ex.Message, ex.Ids });
		}

		return NoContent();
	}
}
