using Markdig;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BoneLog.Blazor.Utilites;

public static class FileReaderHelper
{
    public static (T?, string) ParseMarkdownWithHeader<T>(this string markdown) where T : class
    {
        if(!markdown.StartsWith("---"))
            return (null, markdown);

        int endIndex = markdown.IndexOf("---",3);
        if(endIndex == -1)
            return (null, markdown);

        string yamlContent = markdown.Substring(3,endIndex - 3).Trim();
        string markdownBody = markdown.Substring(endIndex + 3).Trim();

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var header = deserializer.Deserialize<T>(yamlContent);

        return (header, markdownBody);
    }

    public static string MarkdownToHtml(this string markdown)
    {
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        return Markdown.ToHtml(markdown,pipeline).ApplyAutoDirection();
    }
    public static string RemoveYamlHeader(this string markdown)
    {
        if(markdown.StartsWith("---"))
        {
            int start = markdown.IndexOf("---");
            int end = markdown.IndexOf("---",start + 3);
            markdown = (start != -1 && end != -1) ? markdown.Substring(end + 3).TrimStart() : markdown;
        }
        return markdown;
    }

    private static string ApplyAutoDirection(this string html)
    {
        string tags = "p|div|span|h1|h2|h3|h4|h5|h6";
        var regex = new Regex($@"<({tags})(?![^>]*dir=)([^>]*)>(\s*[\u0600-\u06FF])",RegexOptions.IgnoreCase | RegexOptions.Compiled);
        return regex.Replace(html,@"<$1 dir=""rtl""$2>$3");
    }

    public static (T?, string) ParseMarkdownToHtmlWithHeader<T>(this string markdown) where T : class
    {
        if(!markdown.StartsWith("---"))
            return (null, markdown);

        int endIndex = markdown.IndexOf("---",3);
        if(endIndex == -1)
            return (null, markdown);

        string yamlContent = markdown.Substring(3,endIndex - 3).Trim();
        string markdownBody = markdown.Substring(endIndex + 3).Trim();
        markdownBody = MarkdownToHtml(markdownBody);

        var deserializer = new DeserializerBuilder()
          .WithNamingConvention(CamelCaseNamingConvention.Instance)
          .IgnoreUnmatchedProperties()
          .Build();

        var header = deserializer.Deserialize<T>(yamlContent);
        return (header, markdownBody);
    }
}
