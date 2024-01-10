using Domain.DTOs;
using Domain.DTOs.Products;
using Domain.Entities;
using Domain.Mappers;
using MAUIApp.Services.Interfaces;

namespace MAUIApp.Services.Mocks;
public class ProductService : IProductService
{
	private readonly List<Product> _products =
	[
		new Product()
		{
			Id = "12a4",
			Barcode = "8935006538986",
			Name = "VROHTO Mineral Tear",
			DateCreated = DateTime.Now,
			BuyingPrice = 60000,
			SellingPrice = 72000,
			InStock = 13,
			Brand = "VROHTO",
			Group = "Nhỏ mắt",
			Description = "Dung dịch nhỏ mắt giữ ẩm và bổ sung khoáng chất",
			StoragePosition = "Kệ 1 hàng 3"
		},
		new Product()
		{
			Id = "539n",
			Barcode = "8935049902812",
			Name = "Natri clorid 0,9%",
			DateCreated = DateTime.Now,
			BuyingPrice = 8000,
			SellingPrice = 10000,
			InStock = 40,
			Group = string.Empty,
			Brand = string.Empty,
			Description = string.Empty,
			StoragePosition = string.Empty
		},
		new Product()
		{
			Id = "a174",
			Barcode = "8992772065023",
			Name = "Soffell",
			DateCreated = DateTime.Now,
			BuyingPrice = 8000,
			SellingPrice = 53000,
			InStock = 4,
			Group = string.Empty,
			Brand = string.Empty,
			Description = string.Empty,
			StoragePosition = string.Empty
		},
		new Product()
		{
			Id = "12a4",
			Barcode = "8935006538986",
			Name = "VROHTO Mineral Tearrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr",
			DateCreated = DateTime.Now,
			BuyingPrice = 60000,
			SellingPrice = 72000,
			InStock = 13,
			Brand = "VROHTO",
			Group = "Nhỏ mắt",
			Description = "Dung dịch nhỏ mắt giữ ẩm và bổ sung khoáng chất",
			StoragePosition = "Kệ 1 hàng 3"
		},
	];

	public async Task<ProductShort[]> SearchAsync(
		string nameOrBarcode,
		OrderBy orderBy = OrderBy.Ascending,
		ushort pageNumber = 1,
		ushort pageSize = 15)
	{
		var results = _products.Select(ProductMapper.ToShort).ToArray();

		return await Task.FromResult(results);
	}

	public async Task<Product> GetAsync(string id)
	{
		var result = _products.FirstOrDefault(x => x.Id == id);
		await Task.Delay(2000);
		if (result == null || result.Id == "a174")
		{
			await Task.Delay(10000);
			throw new ArgumentException(id);
		}

		return await Task.FromResult(result);
	}

	public Task CreateAsync(ProductCreateUpdate data)
	{
		return Task.CompletedTask;
	}

	public Task UpdateAsync(string id, ProductCreateUpdate data)
	{
		return Task.CompletedTask;
	}

	public Task DeleteAsync(string id)
	{
		return Task.CompletedTask;
	}
}
