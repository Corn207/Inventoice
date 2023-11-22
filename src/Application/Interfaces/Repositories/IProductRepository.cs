using Application.Interfaces.Repositories.Bases;
using Application.Interfaces.Repositories.Parameters;
using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IProductRepository : IRepository<Product>
{
	Task<List<Product>> SearchAsync(
		string nameOrBarcode,
		PaginationParameters pagination,
		ProductOrderByParameter orderBy,
		bool isDescending);
}
