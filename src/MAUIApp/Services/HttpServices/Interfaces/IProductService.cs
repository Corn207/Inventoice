using Domain.DTOs;
using Domain.DTOs.Products;
using Domain.Entities;

namespace MAUIApp.Services.HttpServices.Interfaces;
public interface IProductService
{
	Task<IEnumerable<ProductShort>> SearchAsync(
		string? nameOrBarcode = null,
		OrderBy orderBy = OrderBy.Ascending,
		ushort pageNumber = 1,
		ushort pageSize = 15,
		CancellationToken cancellationToken = default);

	Task<uint> TotalAsync(
		CancellationToken cancellationToken = default);

	Task<Product> GetAsync(
		string id,
		CancellationToken cancellationToken = default);

	Task CreateAsync(
		ProductCreateUpdate body,
		CancellationToken cancellationToken = default);

	Task UpdateAsync(
		string id,
		ProductCreateUpdate body,
		CancellationToken cancellationToken = default);

	Task DeleteAsync(
		string id,
		CancellationToken cancellationToken = default);
}
