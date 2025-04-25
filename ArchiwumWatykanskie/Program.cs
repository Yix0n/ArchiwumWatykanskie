using ArchiwumWatykanskie.Data;
using Microsoft.EntityFrameworkCore;

namespace ArchiwumWatykanskie;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlite("Data Source=app.db");
        });

        var app = builder.Build();
        
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated();
        }
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseRouting();
        
        app.UseAuthorization();

        app.MapControllers();
        
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            var dataSource = app.Services.GetRequiredService<EndpointDataSource>();
            foreach (var endpoint in dataSource.Endpoints)
            {
                if (endpoint is RouteEndpoint routeEndpoint)
                {
                    Console.WriteLine($"[ROUTE] {routeEndpoint.RoutePattern.RawText}");
                }
            }
        });


        app.Run();
    }
}