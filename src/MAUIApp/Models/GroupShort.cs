namespace MAUIApp.Models;
public record GroupShort<T>
{
	public required DateOnly Date { get; init; }
	public required IReadOnlyList<T> Shorts { get; init; }
}
