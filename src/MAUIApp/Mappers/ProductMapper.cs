using Domain.DTOs.Products;
using Domain.Entities;
using MAUIApp.Models.Products;

namespace MAUIApp.Mappers;
public static class ProductMapper
{
	public static ProductItem ToItem(ProductShort source)
	{
		var result = new ProductItem()
		{
			Id = source.Id,
			Name = source.Name,
			Barcode = source.Barcode,
			InStock = source.InStock,
			Price = source.Price
		};

		return result;
	}

	public static ProductDetails ToDetails(Product source)
	{
		if (string.IsNullOrWhiteSpace(source.Id))
		{
			throw new ArgumentException($"Id is empty while mapping {nameof(Product)} to {nameof(ProductDetails)}.");
		}

		var target = new ProductDetails()
		{
			Name = source.Name,
			Barcode = source.Barcode,
			Group = source.Group,
			Brand = source.Brand,
			StoragePosition = source.StoragePosition,
			Description = source.Description,
			BuyingPrice = source.BuyingPrice,
			SellingPrice = source.SellingPrice,
			Id = source.Id,
			InStock = source.InStock,
			DateCreated = source.DateCreated,
		};

		return target;
	}

	public static ProductCreateUpdate ToCreateUpdate(ProductDetails source)
	{
		var target = new ProductCreateUpdate()
		{
			Name = source.Name,
			Barcode = source.Barcode,
			Group = source.Group,
			Brand = source.Brand,
			StoragePosition = source.StoragePosition,
			Description = source.Description,
			BuyingPrice = source.BuyingPrice,
			SellingPrice = source.SellingPrice
		};

		return target;
	}
}
