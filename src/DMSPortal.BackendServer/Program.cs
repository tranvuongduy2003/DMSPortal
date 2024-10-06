using DMSPortal.BackendServer.Extensions;
using Serilog;

var AppCors = "AppCors";

var builder = WebApplication.CreateBuilder(args);

Log.Information("Starting DMS Portal API up");

try
{
    builder.Host.UseSerilog(LoggingExtensions.Configure);
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    builder.Host.AddAppConfigurations();

    builder.Services.AddInfrastructure(builder.Configuration, AppCors);

    var app = builder.Build();

    app.UseInfrastructure(AppCors);

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information("Shut down DMS Portal API complete");
    Log.CloseAndFlush();
}