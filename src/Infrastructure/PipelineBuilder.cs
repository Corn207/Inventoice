using Domain.DTOs;
using Domain.Entities;
using Domain.Entities.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Infrastructure;
internal class PipelineBuilder<T>
{
	private PipelineDefinition<T, T> _pipeline = new EmptyPipelineDefinition<T>();
	private readonly List<FilterDefinition<T>> _filters = [];
	private SortDefinition<T>? _sort;
	private Pagination? _pagination;

	public PipelineBuilder<T> Match(FilterDefinition<T> definition)
	{
		_filters.Add(definition);

		return this;
	}

	public PipelineBuilder<T> MatchOr(params (FieldDefinition<T, string> field, string? term)[] fieldSearchs)
	{
		var filters = new List<FilterDefinition<T>>();
		foreach (var (field, term) in fieldSearchs)
		{
			if (!string.IsNullOrWhiteSpace(term))
			{
				var addition = Builders<T>.Filter
					.Regex(field, new BsonRegularExpression(Regex.Escape(term), "i"));

				filters.Add(addition);
			}
		}

		if (filters.Count == 1)
		{
			_filters.Add(filters[0]);
		}
		else if (filters.Count > 1)
		{
			var or = Builders<T>.Filter.Or(filters);
			_filters.Add(or);
		}
		return this;
	}

	public PipelineBuilder<T> MatchOr<TItem>(
		FieldDefinition<T, IEnumerable<TItem>> fieldItems,
		params (FieldDefinition<TItem, string> field, string? term)[] fieldSearchs)
	{
		var filters = new List<FilterDefinition<TItem>>();
		foreach (var (field, term) in fieldSearchs)
		{
			if (!string.IsNullOrWhiteSpace(term))
			{
				var addition = Builders<TItem>.Filter
					.Regex(field, new BsonRegularExpression(Regex.Escape(term), "i"));

				filters.Add(addition);
			}
		}
		if (filters.Count == 1)
		{
			_filters.Add(Builders<T>.Filter.ElemMatch(fieldItems, filters[0]));
		}
		else if (filters.Count > 1)
		{
			var or = Builders<TItem>.Filter.Or(filters);
			_filters.Add(Builders<T>.Filter.ElemMatch(fieldItems, or));
		}

		return this;
	}

	public PipelineBuilder<T> Match(FieldDefinition<T, DateTime> field, TimeRange timeRange)
	{
		if (timeRange.From != DateTime.MinValue)
		{
			var addition = Builders<T>.Filter.Gte(field, timeRange.From);

			_filters.Add(addition);
		}
		if (timeRange.To != DateTime.MaxValue)
		{
			var addition = Builders<T>.Filter.Lte(field, timeRange.To);

			_filters.Add(addition);
		}

		return this;
	}

	public PipelineBuilder<T> Sort(FieldDefinition<T, object> field, OrderBy orderBy)
	{
		if (orderBy == OrderBy.Ascending)
		{
			_sort = Builders<T>.Sort.Ascending(field);
		}
		else if (orderBy == OrderBy.Descending)
		{
			_sort = Builders<T>.Sort.Descending(field);
		}

		return this;
	}

	public PipelineBuilder<T> Paging(Pagination pagination)
	{
		_pagination = pagination;

		return this;
	}

	public PipelineDefinition<T, T> Build()
	{
		if (_filters.Count == 1)
		{
			_pipeline = _pipeline.Match(_filters[0]);
		}
		else if (_filters.Count > 1)
		{
			var filter = Builders<T>.Filter.And(_filters);
			_pipeline = _pipeline.Match(filter);
		}

		if (_sort is not null)
		{
			_pipeline = _pipeline.Sort(_sort);
		}

		if (_pagination is not null)
		{
			_pipeline = _pipeline
				.Skip((_pagination.Value.PageNumber - 1) * _pagination.Value.PageSize)
				.Limit(_pagination.Value.PageSize);
		}

		return _pipeline;
	}

	public async Task<uint> BuildAndCount(IMongoCollection<T> mongoCollection)
	{
		var pipeline = Build().Count();
		var result = await mongoCollection.Aggregate(pipeline).FirstOrDefaultAsync();
		if (result == null) return 0;
		return Convert.ToUInt32(result.Count);
	}
}
