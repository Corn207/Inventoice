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
	public string? StoragePosition { get; set; }
	public string? Description { get; set; }
	public required uint BuyingPrice { get; set; }
	public required uint SellingPrice { get; set; }
	public required uint InStock { get; set; }
	public required DateTime DateCreated { get; set; }
}
