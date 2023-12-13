using Domain.Entities.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;
public class Invoice : IEntity, ISoftDeletableEntity, ICancellableEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }
	public required UserInfo Author { get; set; }
	public InvoiceClientInfo? Client { get; set; }
	public required string ExportReportId { get; set; }
	public required List<InvoiceProductItem> ProductItems { get; set; }
	public uint GrandTotal { get; set; }
	public DateTime DateCreated { get; set; }
	public DateTime? DatePaid { get; set; }
	public DateTime? DateCancelled { get; set; }
	public DateTime? DateDeleted { get; set; }
}

public class InvoiceClientInfo
{
	public required string ClientId { get; set; }
	public required string Name { get; set; }
	public required string PhoneNumber { get; set; }
}

public class InvoiceProductItem
{
	public required string ProductId { get; set; }
	public required string Name { get; set; }
	public required string Barcode { get; set; }
	public uint UnitPrice { get; set; }
	public uint Quantity { get; set; }
}
