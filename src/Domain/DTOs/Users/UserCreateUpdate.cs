namespace Domain.DTOs.Users;
public readonly record struct UserCreateUpdate(
	string Name,
	string Phonenumber);
