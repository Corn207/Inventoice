using Domain.DTOs.Interfaces;
using Domain.DTOs.Users;

namespace Domain.DTOs.ImportReports;
public readonly record struct ImportReportShort(
	string Id,
	UserShort Author,
	uint TotalProduct,
	uint TotalPrice,
	DateTime DateCreated) : IDto;
