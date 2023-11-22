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
	public required string PhoneNumber { get; set; }
	public string? Email { get; set; }
	public string? Address { get; set; }
	public string? Description { get; set; }
	public ClientGenderType? Gender { get; set; }
	public DateTime DateCreated { get; set; }
}

public enum ClientGenderType
{
	Male, Female
}
