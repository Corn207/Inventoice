namespace Domain.DTOs.Products;
public readonly record struct ProductShort(
	string Id,
	string Name,
	string Barcode,
	uint Price,
	uint InStock);
