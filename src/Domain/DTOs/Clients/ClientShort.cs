namespace Domain.DTOs.Clients;
public record struct ClientShort
{
	public required string Id { get; set; }
	public required string Name { get; set; }
	public required string PhoneNumber { get; set; }
}
