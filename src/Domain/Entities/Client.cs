using Domain.Entities.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;
public class Client : IEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }
	public required string Name { get; set; }
	public required string Phonenumber { get; set; }
	public required DateTime DateCreated { get; set; }
	public string? Email { get; set; }
	public string? Address { get; set; }
	public string? Description { get; set; }
}

public class ClientInfo
{
	public required string Id { get; set; }
	public required string Name { get; set; }
	public required string Phonenumber { get; set; }
}
