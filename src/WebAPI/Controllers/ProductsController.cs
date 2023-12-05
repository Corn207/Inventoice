using Application.Services;
using Domain.DTOs.Products;
using Domain.Entities;
using Domain.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
	private readonly ProductService _productService;

	public ProductsController(ProductService productService)
	{
		_productService = productService;
	}

	[HttpGet]
	public async Task<IEnumerable<ProductShort>> Get(
		[FromQuery] string? search = null,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15,
		[FromQuery] ProductOrderByParameter orderBy = ProductOrderByParameter.Name,
		[FromQuery] bool isDescending = false)
	{
		return await _productService.SearchAsync(search, pageNumber, pageSize, orderBy, isDescending);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Product>> Get(string id)
	{
		var product = await _productService.GetAsync(id);
		if (product is null)
		{
			return NotFound();
		}

		return product;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] ProductCreateUpdate body)
	{
		var newId = await _productService.CreateAsync(body);
		return CreatedAtAction(nameof(Get), new { id = newId }, null);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Put(string id, [FromBody] ProductCreateUpdate body)
	{
		try
		{
			await _productService.ReplaceAsync(id, body);
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

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		try
		{
			await _productService.DeleteAsync(id);
		}
		catch (Exception)
		{
			return NotFound();
		}

		return NoContent();
	}
}
