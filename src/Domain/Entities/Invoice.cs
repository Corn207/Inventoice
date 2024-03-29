﻿using Domain.Entities.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;
public class Invoice : IEntity, ICancellableEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }
	public required UserInfo Author { get; set; }
	public required string ExportReportId { get; set; }
	public required List<InvoiceProductItem> ProductItems { get; set; }
	public required uint GrandTotal { get; set; }
	public required DateTime DateCreated { get; set; }
	public ClientInfo? Client { get; set; }
	public DateTime? DatePaid { get; set; }
	public DateTime? DateCancelled { get; set; }
}

public class InvoiceProductItem
{
	public required string Id { get; set; }
	public required string Name { get; set; }
	public required string Barcode { get; set; }
	public required uint Price { get; set; }
	public required uint Quantity { get; set; }
}
