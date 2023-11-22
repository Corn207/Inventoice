namespace Domain.DTOs.Products;
public record struct ProductShort
{
	public required string Id { get; set; }
	public required string Name { get; set; }
	public required string Barcode { get; set; }
	public uint SellingPrice { get; set; }
	public uint StockCount { get; set; }
}
