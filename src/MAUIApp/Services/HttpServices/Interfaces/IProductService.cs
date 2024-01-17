using Domain.DTOs;
using Domain.DTOs.Products;
using Domain.Entities;

namespace MAUIApp.Services.HttpServices.Interfaces;

public interface IProductService : IService<Product>
{
    Task<ProductShort[]> SearchAsync(
        string? nameOrBarcode = null,
        OrderBy orderBy = OrderBy.Ascending,
        ushort pageNumber = 1,
        ushort pageSize = 15,
		CancellationToken cancellationToken = default);
    Task<uint> CountAsync(
        string? nameOrBarcode = null,
		CancellationToken cancellationToken = default);
    Task CreateAsync(
        ProductCreateUpdate body,
		CancellationToken cancellationToken = default);
    Task UpdateAsync(
        string id,
        ProductCreateUpdate body,
		CancellationToken cancellationToken = default);
}
