using Microsoft.Extensions.DependencyInjection;

namespace CurrieTechnologies.Razor.PageVisibility
{
    public static class ExtensionMethods
    {
        public static IServiceCollection AddPageVisibility(this IServiceCollection services)
        {
            return services.AddScoped<PageVisibilityService>();
        }
    }
}
