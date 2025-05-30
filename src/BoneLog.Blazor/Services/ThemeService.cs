using Microsoft.JSInterop;

namespace BoneLog.Blazor.Services;
public class ThemeService
{
    private readonly IJSRuntime _jsRuntime;

    private const string StorageKey = "isDarkTheme";

    public bool IsDark { get; private set; } = false;

    public event Action? OnChange;

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        var stored = await _jsRuntime.InvokeAsync<string>("localStorageFunctions.getItem",StorageKey);
        if(bool.TryParse(stored,out var isDarkStored))
        {
            IsDark = isDarkStored;
            OnChange?.Invoke();
        }
    }

    public async Task ToggleAsync()
    {
        IsDark = !IsDark;
        await _jsRuntime.InvokeVoidAsync("localStorageFunctions.setItem",StorageKey,IsDark.ToString());
        OnChange?.Invoke();
    }
}

