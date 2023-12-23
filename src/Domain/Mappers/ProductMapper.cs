using Domain.DTOs.Products;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers;
[Mapper]
public static partial class ProductMapper
{
	[MapProperty(nameof(Product.SellingPrice), nameof(ProductShort.Price))]
	public static partial ProductShort ToShort(Product source);

	public static Product ToEntity(ProductCreateUpdate source)
	{
		var target = new Product()
		{
			Name = source.Name,
			Barcode = source.Barcode,
			Group = source.Group,
			Brand = source.Brand,
			StoragePosition = source.StoragePosition,
			Description = source.Description,
			BuyingPrice = source.BuyingPrice,
			SellingPrice = source.SellingPrice,
			InStock = 0,
			DateCreated = DateTime.UtcNow,
		};

		return target;
	}

	public static partial void ToEntity(ProductCreateUpdate source, Product target);
}
