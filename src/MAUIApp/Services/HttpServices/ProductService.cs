using Domain.DTOs;
using Domain.DTOs.Products;
using Domain.Entities;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.Utilities;

namespace MAUIApp.Services.HttpServices;
public class ProductService(HttpService httpService) : IProductService
{
	private const string _path = "Products";

	public async Task<IEnumerable<ProductShort>> SearchAsync(
		string? nameOrBarcode = null,
		OrderBy orderBy = OrderBy.Ascending,
		ushort pageNumber = 1,
		ushort pageSize = 30,
		CancellationToken cancellationToken = default)
	{
		var queries = new Dictionary<string, object?>()
		{
			{ nameof(nameOrBarcode), nameOrBarcode },
			{ nameof(orderBy), orderBy },
			{ nameof(pageNumber), pageNumber },
			{ nameof(pageSize), pageSize },
		};
		var queryString = QueryStringConverter.Convert(queries);
		var uri = new Uri(httpService.BaseUri, $"{_path}/search?{queryString}");
		var results = await httpService.GetAsync<IEnumerable<ProductShort>>(uri, cancellationToken);

		return results;
	}

	public async Task<uint> TotalAsync(
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}/total");
		var results = await httpService.GetAsync<uint>(uri, cancellationToken);

		return results;
	}

	public async Task<Product> GetAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}/{id}");
		var result = await httpService.GetAsync<Product>(uri, cancellationToken);

		return result;
	}

	public async Task CreateAsync(
		ProductCreateUpdate body,
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}");
		await httpService.PostNoContentAsync(uri, body, cancellationToken);
	}

	public async Task UpdateAsync(
		string id,
		ProductCreateUpdate body,
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}/{id}");
		await httpService.PutNoContentAsync(uri, body, cancellationToken);
	}

	public async Task DeleteAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}/{id}");
		await httpService.DeleteAsync(uri, cancellationToken);
	}
}
