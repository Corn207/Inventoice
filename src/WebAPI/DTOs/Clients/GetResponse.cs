using MongoDB.Bson;
using static Core.Entities.Client;

namespace WebAPI.DTOs.Clients;

public record GetResponse
{
	public ObjectId Id { get; set; }
	public required string Name { get; set; }
	public required string PhoneNumber { get; set; }
	public string? Description { get; set; }
	public string? Email { get; set; }
	public string? Address { get; set; }
	public GenderType? Gender { get; set; }
}
