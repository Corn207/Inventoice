using static Core.Entities.Client;

namespace WebAPI.DTOs.Clients;

public record CreateRequest
(
	string Name,
	string PhoneNumber,
	string? Description = null,
	string? Email = null,
	string? Address = null,
	GenderType? Gender = null
);
