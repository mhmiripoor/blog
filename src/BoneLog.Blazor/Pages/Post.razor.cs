using BoneLog.Blazor.Dtos;
using BoneLog.Blazor.Utilites;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BoneLog.Blazor.Pages;

public partial class Post : ComponentBase
{
    [Inject] HttpClient Http { get; set; } = default!;
    [Inject] IJSRuntime JS { get; set; } = default!;
    [Inject] NavigationManager Nav { get; set; } = default!;

    [Parameter]
    public string PostName { get; set; } = null!;

    private bool isLoading = true;
    private bool postExists = false;
    private string htmlContent = string.Empty;
    private PostHeaderDto? header = null;

    protected override async Task OnInitializedAsync()
    {
        var response = await Http.GetAsync($"{Nav.BaseUri}/data/posts/{PostName}.md");
        if(response.IsSuccessStatusCode)
        {
            isLoading = false;
            postExists = true;
            string markdown = await response.Content.ReadAsStringAsync();
            (header, htmlContent) = markdown.ParseMarkdownToHtmlWithHeader<PostHeaderDto>();
        }
        else
        {
            postExists = false;
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
