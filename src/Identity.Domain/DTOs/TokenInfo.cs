namespace Identity.Domain.DTOs;
public readonly record struct TokenInfo(
	string Token,
	string UserId,
	DateTime DateCreated,
	DateTime DateExpired
);
