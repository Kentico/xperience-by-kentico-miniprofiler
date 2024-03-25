using CMS.DataEngine;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Microsoft.Extensions.DependencyInjection;

public static class MiniProfilerServiceCollectionExtensions
{
    public static IServiceCollection AddKenticoMiniProfiler(this IServiceCollection services)
    {
        services.AddMiniProfiler(options =>
            options.TrackConnectionOpenClose = true);
        services.AddTransient<ITagHelperComponent, ScriptTagComponent>();
        services.AddTransient<IDataProvider, MiniprofilerDataProvider>();

        return services;
    }
}
