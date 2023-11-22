namespace Domain.DTOs.Products;
public record struct ProductCreateUpdate
{
	public required string Name { get; set; }
	public required string Barcode { get; set; }
	public string? Group { get; set; }
	public string? Brand { get; set; }
	public uint LastImportedPrice { get; set; }
	public uint SellingPrice { get; set; }
	public uint StockCount { get; set; }
	public string? StoragePosition { get; set; }
	public string? Description { get; set; }
}
