namespace BoneLog.Blazor.Dtos;

public record class AboutHeaderDto
{
    public string Name { get; init; } = null!;
    public string? Headline { get; init; }
    public string? Avatar { get; init; }
}
