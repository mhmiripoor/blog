using BoneLog.Blazor.Dtos;
using System.Net.Http.Json;

namespace BoneLog.Blazor.Services;

public class SiteSettingsService
{
    public SiteSettingsDto Settings { get; private set; }

    private readonly HttpClient _http;

    public SiteSettingsService(HttpClient http)
    {
        _http = http;
    }

    public async Task LoadAsync()
    {
        if(Settings != null)
            return;

        Settings = await _http.GetFromJsonAsync<SiteSettingsDto>($"data/site-settings.json") ?? new("BoneLOG",[],[]);
    }
}
