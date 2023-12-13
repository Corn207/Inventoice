using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.Products;
using Domain.Entities;
using Domain.Mappers;

namespace Application.Services;
public class ProductService(IProductRepository productRepository)
{
	public async Task<IEnumerable<ProductShort>> SearchAsync(
		string nameOrBarcode,
		ushort pageNumber,
		ushort pageSize,
		ProductSortBy sortBy,
		OrderBy orderBy)
	{
		var pagination = new Pagination(pageNumber, pageSize);
		var entities = await productRepository.SearchAsync(nameOrBarcode, pagination, sortBy, orderBy);

		return entities.Select(ProductMapper.ToShortForm);
	}

	public async Task<Product?> GetAsync(string id)
	{
		return await productRepository.GetAsync(id);
	}

	public async Task<string> CreateAsync(ProductCreateUpdate create)
	{
		var entity = ProductMapper.ToEntity(create);
		entity.DateCreated = DateTime.Now;
		await productRepository.CreateAsync(entity);

		return entity.Id!;
	}

	public async Task ReplaceAsync(string id, ProductCreateUpdate update)
	{
		var entity = await productRepository.GetAsync(id) ?? throw new KeyNotFoundException();
		ProductMapper.ToEntity(update, entity);
		entity.DateModified = DateTime.Now;
		await productRepository.ReplaceAsync(entity);
	}

	public async Task DeleteAsync(string id)
	{
		try
		{
			await productRepository.DeleteAsync(id);
		}
		catch (KeyNotFoundException ex)
		{
			throw ex;
		}
	}
}
