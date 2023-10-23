using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Entities;
public class ExportReport
{
	public class ProductItem
	{
		public required string ProductId { get; set; }
		public required string ProductName { get; set; }
		public int Quantity { get; set; }
	}

	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }
	public required Author Author { get; set; }
	public DateTime CreatedTime { get; set; }
	public DateTime? CancelledTime { get; set; }
	public required List<ProductItem> ProductItems { get; set; }
}
