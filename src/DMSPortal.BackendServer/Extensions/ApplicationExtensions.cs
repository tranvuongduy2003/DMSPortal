using DMSPortal.BackendServer.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DMSPortal.BackendServer.Extensions;

public static class ApplicationExtensions
{
    public static void UseInfrastructure(this WebApplication app, string appCors)
    {
        // Configure the HTTP request pipeline.

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{app.Environment.ApplicationName} V1");
        });


        app.UseRouting();
        //app.UseHttpsRedirection(); //production only

        app.UseErrorWrapping();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors(appCors);

        app.UseHangfireDashboard(app.Configuration);

        app.MapControllers();
        app.MapGet("/", context => Task.Run(() =>
            context.Response.Redirect("/swagger/index.html")));

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();
            var context = services.GetRequiredService<ApplicationDbContext>();

            try
            {
                logger.LogInformation("Migrating database.");
                if (context.Database.GetPendingMigrations().Any())
                    context.Database.Migrate();
                logger.LogInformation("Migrated database.");
                Log.Information("Seeding data...");
                var dbInitializer = services.GetService<DbInitializer>();
                dbInitializer?.Seed().Wait();
                Log.Information("Seeding data successfully!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}