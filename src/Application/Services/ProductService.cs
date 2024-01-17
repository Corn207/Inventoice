using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.Products;
using Domain.Entities;
using Domain.Mappers;

namespace Application.Services;
public class ProductService(IProductRepository productRepository)
{
	public async Task<IEnumerable<ProductShort>> SearchAsync(
		string? nameOrBarcode,
		OrderBy orderBy,
		Pagination pagination)
	{
		var entities = await productRepository.SearchAsync(nameOrBarcode, orderBy, pagination);

		return entities.Select(ProductMapper.ToShort);
	}

	public async Task<uint> CountAllAsync()
	{
		var count = await productRepository.CountAllAsync();

		return count;
	}

	public async Task<Product?> GetAsync(string id)
	{
		return await productRepository.GetAsync(id);
	}

	public async Task<string> CreateAsync(ProductCreateUpdate create)
	{
		var entity = ProductMapper.ToEntity(create);
		entity.DateCreated = DateTime.UtcNow;
		await productRepository.CreateAsync(entity);

		return entity.Id!;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="update"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	public async Task ReplaceAsync(string id, ProductCreateUpdate update)
	{
		var entity = await productRepository.GetAsync(id)
			?? throw new NotFoundException("Product's Id was not found.", id);
		ProductMapper.ToEntity(update, entity);
		await productRepository.ReplaceAsync(entity);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	public async Task DeleteAsync(string id)
	{
		await productRepository.DeleteAsync(id);
	}
}
