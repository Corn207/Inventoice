using Domain.DTOs;
using Domain.Entities.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Infrastructure;
internal static class Utility
{
	#region Match
	public static FilterDefinition<T>? TextSearchCaseInsentitive<T>(
		Expression<Func<T, object>> field,
		string? term)
	{
		if (string.IsNullOrWhiteSpace(term)) return null;

		var regex = new BsonRegularExpression(Regex.Escape(term), "i");
		var filter = Builders<T>.Filter.Regex(field, regex);

		return filter;
	}

	public static void AddFilter<T, TField>(
		this IList<FilterDefinition<T>> list,
		Expression<Func<T, TField>> field,
		TField? value)
	{
		if (value is null) return;

		var filter = Builders<T>.Filter.Eq(field, value);
		list.Add(filter);
	}

	public static void AddFilterArrayContainsAnyRegex<T, TItem>(
		this IList<FilterDefinition<T>> list,
		Expression<Func<T, IEnumerable<TItem>>> fieldItems,
		IEnumerable<(Expression<Func<TItem, object>> field, string? term)> fieldRegexes)
	{
		var searchs = fieldRegexes.Where(x => !string.IsNullOrWhiteSpace(x.term)).ToList();
		if (searchs.Count == 0) return;

		var filters = searchs.Select(x => TextSearchCaseInsentitive(x.field, x.term));

		if (searchs.Count == 1)
		{
			var filter = Builders<T>.Filter.ElemMatch(fieldItems, filters.First());
			list.Add(filter);
		}
		else
		{
			var or = Builders<TItem>.Filter.Or(filters);
			var filter = Builders<T>.Filter.ElemMatch(fieldItems, or);
			list.Add(filter);
		}
	}

	public static void AddFilterTimeRange<T>(
		this IList<FilterDefinition<T>> list,
		Expression<Func<T, DateTime>> field,
		DateTime from,
		DateTime to)
	{
		if (from != DateTime.MinValue)
		{
			list.Add(Builders<T>.Filter.Gte(field, from));
		}
		if (to != DateTime.MaxValue)
		{
			list.Add(Builders<T>.Filter.Lte(field, to));
		}
	}

	public static PipelineStageDefinition<T, T>? BuildStageMatchAnd<T>(IList<FilterDefinition<T>> list)
	{
		if (list.Count == 0) return null;
		if (list.Count == 1) PipelineStageDefinitionBuilder.Match(list[0]);

		var and = Builders<T>.Filter.And(list);
		var stage = PipelineStageDefinitionBuilder.Match(and);

		return stage;
	}
	#endregion

	public static PipelineStageDefinition<T, T> BuildStageSort<T>(
		Expression<Func<T, object>> field,
		OrderBy orderBy)
	{
		var sort = orderBy switch
		{
			OrderBy.Ascending => Builders<T>.Sort.Ascending(field),
			_ => Builders<T>.Sort.Descending(field)
		};

		var stage = PipelineStageDefinitionBuilder.Sort(sort);
		
		return stage;
	}

	public static PipelineStageDefinition<TEntity, PartialEnumerable<TEntity>> BuildStageGroupAndPage<TEntity>(
		Pagination pagination) where TEntity : IEntity
	{
		var stage = PipelineStageDefinitionBuilder.Group<TEntity, string, PartialEnumerable<TEntity>>(
			x => x.Id!,
			group => new PartialEnumerable<TEntity>(
				group.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize),
				Convert.ToUInt32(group.Count())));

		return stage;
	}
}
