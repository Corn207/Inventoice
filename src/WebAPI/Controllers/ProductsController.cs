using Application.Exceptions;
using Application.Services;
using Domain.DTOs;
using Domain.DTOs.Products;
using Domain.Entities;
using Identity.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{nameof(Role.Manager)},{nameof(Role.Employee)}")]
public class ProductsController(ProductService service) : ControllerBase
{
	[HttpGet("search")]
	[ProducesResponseType<PartialArray<ProductShort>>(StatusCodes.Status200OK)]
	public async Task<PartialArray<ProductShort>> Search(
		[FromQuery] string? nameOrBarcode = null,
		[FromQuery] OrderBy orderBy = OrderBy.Ascending,
		[FromQuery] ushort pageNumber = 1,
		[FromQuery] ushort pageSize = 15)
	{
		var pagination = new Pagination(pageNumber, pageSize);

		var result = await service.SearchAsync(
			nameOrBarcode,
			orderBy,
			pagination);

		return result;
	}

	[HttpGet("total")]
	[ProducesResponseType<uint>(StatusCodes.Status200OK)]
	public async Task<uint> Total()
	{
		return await service.CountAllAsync();
	}

	[HttpGet("{id}")]
	[ProducesResponseType<Product>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<Product>> Get(string id)
	{
		var product = await service.GetAsync(id)
			?? throw new NotFoundException("Product's Id was not found.", id);

		return product;
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Create([FromBody] ProductCreateUpdate body)
	{
		var newId = await service.CreateAsync(body);

		return CreatedAtAction(nameof(Get), new { id = newId }, null);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update(string id, [FromBody] ProductCreateUpdate body)
	{
		await service.ReplaceAsync(id, body);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete(string id)
	{
		await service.DeleteAsync(id);

		return NoContent();
	}
}
