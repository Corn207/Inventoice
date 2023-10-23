using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Entities;
public class Invoice
{
	public class Client
	{
		public required string ClientId { get; set; }
		public required string Name { get; set; }
		public required string PhoneNumber { get; set; }
	}

	public class ProductItem
	{
		public required string ProductId { get; set; }
		public required string ProductName { get; set; }
		public int UnitPrice { get; set; }
		public int Quantity { get; set; }
	}

	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }
	public required Author Author { get; set; }
	public required Client Recipient { get; set; }
	public DateTime CreatedTime { get; set; }
	public DateTime? CancelledTime { get; set; }
	public DateTime? PaidTime { get; set; }
	public required List<ProductItem> ProductItems { get; set; }
}
