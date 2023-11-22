using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Parameters;
using Domain.DTOs.Products;
using Domain.Entities;
using Domain.Mappers;

namespace Application.Services;
public class ProductService
{
	private readonly IProductRepository _productRepository;

	public ProductService(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}

	public async Task<IEnumerable<ProductShort>> SearchAsync(
		string? nameOrBarcode = null,
		ushort pageNumber = 1,
		ushort pageSize = 15,
		ProductOrderByParameter orderBy = ProductOrderByParameter.Name,
		bool isDescending = false)
	{
		var pagination = new PaginationParameters(pageNumber, pageSize);
		var entities = await _productRepository.SearchAsync(nameOrBarcode ?? string.Empty, pagination, orderBy, isDescending);

		return entities.Select(ProductMapper.ToShortForm);
	}

	public async Task<Product?> GetAsync(string id)
	{
		return await _productRepository.GetAsync(id);
	}

	public async Task<string> CreateAsync(ProductCreateUpdate create)
	{
		var entity = ProductMapper.ToEntity(create);
		entity.DateCreated = DateTime.Now;
		await _productRepository.CreateAsync(entity);

		return entity.Id!;
	}

	public async Task ReplaceAsync(string id, ProductCreateUpdate update)
	{
		var entity = await _productRepository.GetAsync(id) ?? throw new KeyNotFoundException();
		ProductMapper.ToEntity(update, entity);
		entity.DateModified = DateTime.Now;
		await _productRepository.ReplaceAsync(entity);
	}

	public async Task DeleteAsync(string id)
	{
		try
		{
			await _productRepository.DeleteAsync(id);
		}
		catch (KeyNotFoundException ex)
		{
			throw ex;
		}
	}
}
