namespace BoneLog.Blazor.Dtos;


public record NavItemDto(string Title,string Url);
public record SocialLinkDto(string Url,string IconClass);
public record SiteSettingsDto(string Title,List<NavItemDto>? NavItems,List<SocialLinkDto>? SocialLinks);
