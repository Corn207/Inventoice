using Identity.Domain.Entity;

namespace Identity.Domain.DTOs;
public readonly record struct TokenInfo(
	string Token,
	string UserId,
	Role Roles,
	DateTime DateCreated,
	DateTime DateExpired);
