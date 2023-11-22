using Domain.Entities;

namespace Domain.DTOs.Clients;
public record struct ClientCreateUpdate
{
	public required string Name { get; set; }
	public required string PhoneNumber { get; set; }
	public string? Description { get; set; }
	public string? Email { get; set; }
	public string? Address { get; set; }
	public ClientGenderType? Gender { get; set; }
}
