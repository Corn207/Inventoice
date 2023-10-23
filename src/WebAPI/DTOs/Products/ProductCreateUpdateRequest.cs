namespace WebAPI.DTOs.Products;

public record ProductCreateUpdateRequest
{
	public required string Barcode { get; init; }
	public required string Name { get; init; }
	public string? Group { get; init; }
	public string? Brand { get; init; }
	public int LastImportedPrice { get; init; }
	public int SellingPrice { get; init; }
	public int StockCount { get; init; }
	public string? StoragePosition { get; init; }
	public string? Description { get; init; }
}
