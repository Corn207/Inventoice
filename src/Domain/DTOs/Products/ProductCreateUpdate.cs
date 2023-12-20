namespace Domain.DTOs.Products;
public readonly record struct ProductCreateUpdate(
	string Name,
	string Barcode,
	string? Group,
	string? Brand,
	string? StoragePosition,
	string? Description,
	uint BuyingPrice,
	uint SellingPrice);
