using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

using StackExchange.Profiling;

namespace Kentico.Xperience.MiniProfiler;

/// <summary>
/// Tag component logic for custom script injection into the body element.
/// </summary>
internal class ScriptTagComponent : TagHelperComponent
{
    /// <summary>
    /// Gets the ViewContext.
    /// </summary>
    [ViewContext]
    public ViewContext ViewContext { get; set; } = new ViewContext();


    /// <summary>
    /// Adds the miniprofiler script into the body element.
    /// </summary>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (string.Equals(context.TagName, "body", StringComparison.OrdinalIgnoreCase))
        {
            var outputScript = new TagHelperOutput("__xp_miniprofiler", [], (_, _) => throw new NotImplementedException());
            var tag = new MiniProfilerScriptTagHelper
            {
                ViewContext = ViewContext
            };

            tag.Process(context, outputScript);
            output.PostContent.AppendHtml(outputScript.Content.GetContent());
        }
    }

}
