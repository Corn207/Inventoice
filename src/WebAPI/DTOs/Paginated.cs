namespace WebAPI.DTOs;

public record Paginated<T> where T : class
{
	public IEnumerable<T> Values { get; set; }
	public int PageSize { get; set; }
	public int PageNumber { get; set; }
}
