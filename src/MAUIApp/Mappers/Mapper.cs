using Domain.DTOs.Interfaces;
using MAUIApp.Models;
using MAUIApp.Services;

namespace MAUIApp.Mappers;
public static class Mapper
{
	public static IEnumerable<GroupingByDate<T>> ToGroupingByDateCreated<T>(IEnumerable<T> source)
		where T : IDto
	{
		var target = source
			.GroupBy(
				x => LocalizationService.ToLocalTime(x.DateCreated).Date,
				(key, value) => new GroupingByDate<T>(key, value));

		return target;
	}
}
