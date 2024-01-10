namespace Identity.Domain.DTOs;
public readonly record struct Onboarding(
	string Name,
	string Phonenumber,
	string Username,
	string Password);
