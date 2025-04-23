using ArchiwumWatykanskie.Data;

namespace ArchiwumWatykanskie;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        using (var context = new AppDbContext())
        {
            context.EnsureCreated();
        }

        var app = builder.Build();
        
        
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