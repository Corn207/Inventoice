namespace Identity.Domain.DTOs;
public readonly record struct Login(
	string Username,
	string Password);
