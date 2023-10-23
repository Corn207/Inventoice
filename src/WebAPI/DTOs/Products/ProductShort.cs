namespace WebAPI.DTOs.Products;

public record ProductShort
{
	public required string Id { get; init; }
	public required string Barcode { get; init; }
	public required string Name { get; init; }
	public int SellingPrice { get; init; }
	public int StockCount { get; init; }
}
