using CMS.DataEngine;
using Kentico.Xperience.MiniProfiler;
using Microsoft.AspNetCore.Razor.TagHelpers;
using StackExchange.Profiling;

namespace Microsoft.Extensions.DependencyInjection;

public static class MiniProfilerServiceCollectionExtensions
{
    /// <summary>
    /// Adds Xperience dependencies for MiniProfiler integration, setting required options for MiniProfiler
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddKenticoMiniProfiler(this IServiceCollection services)
    {
        services.AddMiniProfiler(options =>
            options.TrackConnectionOpenClose = true);
        services.AddTransient<ITagHelperComponent, ScriptTagComponent>();
        services.AddTransient<IDataProvider, MiniprofilerDataProvider>();

        return services;
    }

    /// <summary>
    /// Adds Xperience dependencies for MiniProfiler, enabling customization of MiniProfiler and setting required options 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddKenticoMiniProfiler(this IServiceCollection services, Action<MiniProfilerOptions> configureOptions)
    {
        services.AddMiniProfiler(options =>
        {
            configureOptions(options);
            options.TrackConnectionOpenClose = true;
        });
        services.AddTransient<ITagHelperComponent, ScriptTagComponent>();
        services.AddTransient<IDataProvider, MiniprofilerDataProvider>();

        return services;
    }
}
