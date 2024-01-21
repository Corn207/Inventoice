namespace Identity.Domain.DTOs;
public readonly record struct ChangePassword(
	string OldPassword,
	string NewPassword
);
