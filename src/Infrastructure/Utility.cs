using Domain.DTOs;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Infrastructure;
internal static class Utility
{
	public static FilterDefinition<T>? TextSearchCaseInsentitive<T>(
		Expression<Func<T, object>> field,
		string? term)
	{
		if (string.IsNullOrWhiteSpace(term))
		{
			return null;
		}

		var regex = new BsonRegularExpression(Regex.Escape(term), "i");
		var filter = Builders<T>.Filter.Regex(field, regex);

		return filter;
	}

	public static void AddTextSearchCaseInsentitive<T>(
		this IList<FilterDefinition<T>> list,
		params (Expression<Func<T, object>> field, string? term)[] searches)
	{
		if (searches.Length == 0)
		{
			return;
		}
		else if (searches.Length == 1)
		{
			var (field, term) = searches.First();
			var filter = TextSearchCaseInsentitive(field, term);
			if (filter is not null)
			{
				list.Add(filter);
			}
			return;
		}
		else
		{
			var filters = searches
				.Where(x => !string.IsNullOrWhiteSpace(x.term))
				.Select(x => TextSearchCaseInsentitive(x.field, x.term)!)
				.ToList();

			if (filters.Count == 0)
			{
				return;
			}
			else if (filters.Count == 1)
			{
				list.Add(filters.First());
				return;
			}
			else
			{
				var or = Builders<T>.Filter.Or(filters);
				list.Add(or);
				return;
			}
		}
	}

	public static void AddTextSearchCaseInsentitive<T, TItem>(
		this IList<FilterDefinition<T>> list,
		Expression<Func<T, IEnumerable<TItem>>> fieldItems,
		params (Expression<Func<TItem, object>> field, string? term)[] searches)
	{
		if (searches.Length == 0)
		{
			return;
		}
		else if (searches.Length == 1)
		{
			var (field, term) = searches.First();
			var regexFilter = TextSearchCaseInsentitive(field, term);
			if (regexFilter is not null)
			{
				var filter = Builders<T>.Filter.ElemMatch(fieldItems, regexFilter);
				list.Add(filter);
			}
			return;
		}
		else
		{
			var filters = searches
				.Where(x => !string.IsNullOrWhiteSpace(x.term))
				.Select(x => TextSearchCaseInsentitive(x.field, x.term)!)
				.ToList();

			if (filters.Count == 0)
			{
				return;
			}
			else if (filters.Count == 1)
			{
				var filter = Builders<T>.Filter.ElemMatch(fieldItems, filters.First());
				list.Add(filter);
				return;
			}
			else
			{
				var or = Builders<TItem>.Filter.Or(filters);
				var filter = Builders<T>.Filter.ElemMatch(fieldItems, or);
				list.Add(filter);
				return;
			}
		}
	}

	public static void AddFilterTimeRange<T>(
		this IList<FilterDefinition<T>> list,
		Expression<Func<T, DateTime>> field,
		TimeRange timeRange)
	{
		if (timeRange.From != DateTime.MinValue)
		{
			list.Add(Builders<T>.Filter.Gte(field, timeRange.From));
		}
		if (timeRange.To != DateTime.MaxValue)
		{
			list.Add(Builders<T>.Filter.Lte(field, timeRange.To));
		}
	}


	public static IFindFluent<T, T> And<T>(
		this IMongoCollection<T> collection,
		IList<FilterDefinition<T>> filters)
	{
		FilterDefinition<T> filter;
		if (filters.Count == 0)
		{
			filter = Builders<T>.Filter.Empty;
		}
		else if (filters.Count == 1)
		{
			filter = filters[0];
		}
		else
		{
			filter = Builders<T>.Filter.And(filters);
		}

		return collection.Find(filter);
	}

	public static IFindFluent<T, T> Sort<T>(
		this IFindFluent<T, T> collection,
		Expression<Func<T, object>> field,
		OrderBy orderBy)
	{
		var sort = orderBy switch
		{
			OrderBy.Ascending => Builders<T>.Sort.Ascending(field),
			_ => Builders<T>.Sort.Descending(field)
		};

		return collection.Sort(sort);
	}

	public static IFindFluent<T, T> Paginate<T>(
		this IFindFluent<T, T> collection,
		Pagination pagination)
	{
		return collection
			.Skip((pagination.PageNumber - 1) * pagination.PageSize)
			.Limit(pagination.PageSize);
	}
}
