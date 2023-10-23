using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Entities;
public class Client
{
	public enum GenderType
	{
		Male, Female
	}

	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }
	public required string Name { get; set; }
	public required string PhoneNumber { get; set; }
	public string? Description { get; set; }
	public string? Email { get; set; }
	public string? Address { get; set; }
	public GenderType? Gender { get; set; }
}
