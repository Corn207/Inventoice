using Core.Entities;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs.Products;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
	private readonly ProductRepository _productRepository;

	public ProductsController(ProductRepository productRepository)
	{
		_productRepository = productRepository;
	}

	[HttpGet]
	public async Task<IEnumerable<ProductShort>> Search(
		[FromQuery] string? search = null,
		[FromQuery] int pageSize = 15,
		[FromQuery] int pageNumber = 1,
		[FromQuery] ProductRepository.OrderedBy orderedBy = ProductRepository.OrderedBy.Name,
		[FromQuery] bool isDescending = false)
	{
		var pagination = new PaginationParameters(pageSize, pageNumber);
		var products = await _productRepository.SearchAsync(
			pagination,
			search,
			orderedBy,
			isDescending);

		var dtos = products.Select(x => new ProductShort()
		{
			Id = x.Id!,
			Barcode = x.Barcode,
			Name = x.Name,
			SellingPrice = x.SellingPrice,
			StockCount = x.StockCount,
		});

		return dtos;
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Product>> Get(string id)
	{
		var product = await _productRepository.GetAsync(id);
		if (product is null)
		{
			return NotFound();
		}

		return product;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] ProductCreateUpdateRequest body)
	{
		var product = new Product()
		{
			Barcode = body.Barcode,
			Name = body.Name,
			Group = body.Group,
			Brand = body.Brand,
			LastImportedPrice = body.LastImportedPrice,
			SellingPrice = body.SellingPrice,
			StockCount = body.StockCount,
			StoragePosition = body.StoragePosition,
			Description	= body.Description,
			DateCreated = DateTime.Now
		};

		var newId = await _productRepository.CreateAsync(product);
		return CreatedAtAction(nameof(Get), new { id = newId }, product);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Put(string id, [FromBody] ProductCreateUpdateRequest body)
	{
		var product = new Product()
		{
			Id = id,
			Barcode = body.Barcode,
			Name = body.Name,
			Group = body.Group,
			Brand = body.Brand,
			LastImportedPrice = body.LastImportedPrice,
			SellingPrice = body.SellingPrice,
			StockCount = body.StockCount,
			StoragePosition = body.StoragePosition,
			Description = body.Description,
			DateCreated = DateTime.Now
		};

		var isSuccess = await _productRepository.ReplaceAsync(id, product);
		if (isSuccess)
		{
			return NoContent();
		}
		else
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		var result = await _productRepository.DeleteAsync(id);
		if (result)
		{
			return NoContent();
		}
		else
		{
			return NotFound();
		}
	}
}
