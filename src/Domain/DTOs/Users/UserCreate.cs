namespace Domain.DTOs.Users;
public readonly record struct UserCreate(
	string Name,
	string Phonenumber,
	string Username,
	string Password);
