using Identity.Domain.Entity;

namespace Identity.Domain.DTOs;
public readonly record struct CreateIdentity(
	string Name,
	string Phonenumber,
	string Username,
	Role Roles);
