using Identity.Domain.Entity;

namespace Identity.Domain.DTOs;
public readonly record struct IdentityDetails(
	string UserId,
	string Username,
	Role[] Roles
);
