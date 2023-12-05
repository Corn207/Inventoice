using Application.Interfaces.Repositories.Bases;
using Domain.Entities;
using Domain.Parameters;

namespace Application.Interfaces.Repositories;
public interface IProductRepository : IRepository<Product>
{
	Task<List<Product>> SearchAsync(
		string nameOrBarcode,
		PaginationParameters pagination,
		ProductOrderByParameter orderBy,
		bool isDescending);
}
