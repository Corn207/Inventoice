namespace Domain.DTOs.Clients;
public readonly record struct ClientCreateUpdate(
	string Name,
	string Phonenumber,
	string? Email,
	string? Address,
	string? Description);
