using Domain.DTOs;
using Domain.DTOs.Products;
using Domain.Entities;
using MAUIApp.Services.HttpServices.Interfaces;

namespace MAUIApp.Services.HttpServices;
public class ProductService(HttpService httpService) : BaseService<Product> , IProductService
{
	protected override HttpService HttpService => httpService;
	protected override string Path { get; } = "Products";

	public async Task<ProductShort[]> SearchAsync(
		string? nameOrBarcode = null,
		OrderBy orderBy = OrderBy.Ascending,
		ushort pageNumber = 1,
		ushort pageSize = 15,
		CancellationToken cancellationToken = default)
	{
		var queries = new Dictionary<string, object?>()
		{
			{ nameof(nameOrBarcode), nameOrBarcode },
			{ nameof(orderBy), orderBy },
			{ nameof(pageNumber), pageNumber },
			{ nameof(pageSize), pageSize },
		};
		var results = await httpService.GetAsync<ProductShort[]>(Path, queries, cancellationToken);
		return results;
	}

	public async Task<uint> CountAsync(
		string? nameOrBarcode = null,
		CancellationToken cancellationToken = default)
	{
		var queries = new Dictionary<string, object?>()
		{
			{ nameof(nameOrBarcode), nameOrBarcode },
		};
		var results = await httpService.GetAsync<uint>($"{Path}/count", queries, cancellationToken);
		return results;
	}

	public async Task CreateAsync(
		ProductCreateUpdate data,
		CancellationToken cancellationToken = default)
	{
		await httpService.PostNoContentAsync(Path, data, cancellationToken);
	}

	public async Task UpdateAsync(
		string id,
		ProductCreateUpdate data,
		CancellationToken cancellationToken = default)
	{
		await httpService.PutNoContentAsync($"{Path}/{id}", data, cancellationToken);
	}
}
