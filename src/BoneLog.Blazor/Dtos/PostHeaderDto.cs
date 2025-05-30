namespace BoneLog.Blazor.Dtos;

public record class PostHeaderDto
{
    public string Title { get; init; } = null!;
    public string Date { get; init; } = null!;
    public List<string>? Tags { get; init; }
    public string? Cover { get; init; }
}
