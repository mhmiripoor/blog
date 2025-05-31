namespace BoneLog.Blazor.Dtos;


public record NavItemDto(string Title,string Url);
public record SiteSettingsDto(string Title,List<NavItemDto> NavItems);
