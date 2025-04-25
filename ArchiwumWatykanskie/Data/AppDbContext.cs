using ArchiwumWatykanskie.Models;
using Microsoft.EntityFrameworkCore;

namespace ArchiwumWatykanskie.Data;

public class AppDbContext : DbContext
{
    public DbSet<Pope> Popes { get; set; }
    public DbSet<Item> Items { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public AppDbContext() : base(new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite("Data Source=app.db")
        .Options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=app.db");   
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pope>()
            .HasMany(p => p.Items)
            .WithOne(i => i.Pope)
            .HasForeignKey(i => i.PopeId);
    }

    public void EnsureCreated()
    {
        using (var context = new AppDbContext())
        {
            context.Database.EnsureCreated();
        }
    }
}