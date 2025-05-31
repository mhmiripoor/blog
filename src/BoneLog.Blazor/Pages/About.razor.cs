using BoneLog.Blazor.Dtos;
using BoneLog.Blazor.Utilites;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BoneLog.Blazor.Pages;
public partial class About : ComponentBase
{
    [Inject] HttpClient Http { get; set; } = default!;
    [Inject] IJSRuntime JS { get; set; } = default!;

    [Parameter]
    public string PostName { get; set; } = null!;

    private bool isLoading = true;
    private bool aboutExists = false;
    private string htmlContent = string.Empty;
    private AboutHeaderDto? header = null;

    protected override async Task OnInitializedAsync()
    {
        var response = await Http.GetAsync($"data/About.md");
        if(response.IsSuccessStatusCode)
        {
            isLoading = false;
            aboutExists = true;
            string markdown = await response.Content.ReadAsStringAsync();
            (header, htmlContent) = markdown.ParseMarkdownToHtmlWithHeader<AboutHeaderDto>();
        }
        else
        {
            aboutExists = false;
            isLoading = false;
        }
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(htmlContent != null)
        {
            await JS.InvokeVoidAsync("Prism.highlightAll");
        }
    }
}
