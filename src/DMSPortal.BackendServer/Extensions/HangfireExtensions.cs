using DMSPortal.Models.Configurations;
using Hangfire;

namespace DMSPortal.BackendServer.Extensions;

public static class HangfireExtensions
{
    internal static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
    {
        var configureDashboard = configuration.GetSection("HangfireSettings:Dashboard").Get<DashboardOptions>();
        var hangfireSettings = configuration.GetSection("HangfireSettings").Get<HangfireSettings>();
        var hangfireRoute = hangfireSettings?.Route;

        app.UseHangfireDashboard(hangfireRoute, new DashboardOptions
        {
            // Authorization = new [] { },
            DashboardTitle = configureDashboard?.DashboardTitle,
            StatsPollingInterval = configureDashboard?.StatsPollingInterval ?? 0,
            AppPath = configureDashboard?.AppPath,
            IgnoreAntiforgeryToken = true
        });
        
        return app;
    }
}