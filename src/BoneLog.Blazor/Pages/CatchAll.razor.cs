using BoneLog.Blazor.Utilites;
using Microsoft.AspNetCore.Components;

namespace BoneLog.Blazor.Pages;

public partial class CatchAll : ComponentBase
{
    [Inject] HttpClient Http { get; set; } = default!;
    [Inject] NavigationManager Nav { get; set; } = default!;

    [Parameter]
    public string slug { get; set; } = null!;

    private string htmlContent = string.Empty;
    private bool isLoading = true;
    private bool notFound = false;

    protected override async Task OnInitializedAsync()
    {
        var filePath = $"{Nav.BaseUri}/{slug}.md";

        try
        {
            var response = await Http.GetAsync(filePath);
            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                htmlContent = content.RemoveYamlHeader().MarkdownToHtml();
            }
            else
            {
                notFound = true;
            }
        }
        catch
        {
            notFound = true;
        }

        isLoading = false;
    }
}
