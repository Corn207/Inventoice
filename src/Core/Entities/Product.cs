using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Entities;
public class Product
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }
	public required string Barcode { get; set; }
	public required string Name { get; set; }
	public string? Group { get; set; }
	public string? Brand { get; set; }
	public int LastImportedPrice { get; set; }
	public int SellingPrice { get; set; }
	public int StockCount { get; set; }
	public string? StoragePosition { get; set; }
	public string? Description { get; set; }
	public DateTime DateCreated { get; set; }
}
