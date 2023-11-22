using Domain.Entities.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;
public class Product : IEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }
	public required string Name { get; set; }
	public required string Barcode { get; set; }
	public string? Group { get; set; }
	public string? Brand { get; set; }
	public uint LastImportedPrice { get; set; }
	public uint SellingPrice { get; set; }
	public uint StockCount { get; set; }
	public string? StoragePosition { get; set; }
	public string? Description { get; set; }
	public DateTime DateCreated { get; set; }
	public DateTime DateModified { get; set; }
}
