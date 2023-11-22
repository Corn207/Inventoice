using Domain.Entities.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;
public class ImportReport : IEntity, ISoftDeletableEntity, ICancellableEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }
	public required UserInfo Author { get; set; }
	public required List<ImportReportProductItem> ProductItems { get; set; }
	public DateTime DateCreated { get; set; }
	public DateTime? DateCancelled { get; set; }
	public DateTime? DateDeleted { get; set; }
}

public class ImportReportProductItem
{
	public required string ProductId { get; set; }
	public required string Name { get; set; }
	public required string Barcode { get; set; }
	public uint UnitPrice { get; set; }
	public uint Quantity { get; set; }
}
