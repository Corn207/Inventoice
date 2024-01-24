﻿using Domain.Entities.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;
public class ExportReport : IEntity, ICancellableEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }
	public required UserInfo Author { get; set; }
	public required List<ExportReportProductItem> ProductItems { get; set; }
	public required DateTime DateCreated { get; set; }
	public string? InvoiceId { get; set; }
	public DateTime? DateCancelled { get; set; }
}

public class ExportReportProductItem
{
	public required string Id { get; set; }
	public required string Name { get; set; }
	public required string Barcode { get; set; }
	public required uint Quantity { get; set; }
}
