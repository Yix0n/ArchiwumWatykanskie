using ArchiwumWatykanskie.Controllers;
using ArchiwumWatykanskie.Controllers.Requests;
using ArchiwumWatykanskie.Controllers.Responses;
using ArchiwumWatykanskie.Data;
using ArchiwumWatykanskie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ArchiwumWatykanskie.test;

public class PopeControllerTests : IAsyncLifetime
{
    public Task InitializeAsync()
    {
        // Initialize the in-memory database
        var context = GetInMemoryDbContext();
        context.Database.EnsureCreated();
        
        var testPope = new Pope
        {
            Name = "Test Pope",
            BirthDate = DateTime.Now.AddYears(-50),
            DeathDate = DateTime.Now.AddYears(-10)
        };
        
        var testItem = new Item
        {
            Name = "Test Item",
            Description = "Test Description",
            Pope = testPope
        };
        
        context.Popes.Add(testPope);
        context.Items.Add(testItem);
        context.SaveChanges();
        
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        GetInMemoryDbContext().Database.EnsureDeletedAsync();
        
        return Task.CompletedTask;
    }

    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        var context = new AppDbContext(options);
        context.Database.OpenConnection();
        return context;
    }

    [Fact]
    public void CreatePope_ShouldReturnCreatedPope()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new PopeController(context);
        var input = new popeReq_create
        {
            name = "Test Pope"
        };

        // Act
        var result = controller.CreatePope(input);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdPope = Assert.IsType<popeRes_create>(actionResult.Value);
        Assert.Equal("Test Pope", createdPope.Name);
    }

    [Fact]
    public void GetPope_ShouldReturnPope_WhenPopeExists()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var pope = new Pope { Name = "Test Pope" };
        context.Popes.Add(pope);
        context.SaveChanges();

        var controller = new PopeController(context);

        // Act
        var result = controller.GetPope(pope.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<popeRes_get>>(result);
        Assert.NotNull(result.Value);
        Assert.Equal("Test Pope", result.Value.Name);
    }

    [Fact]
    public void UpdatePope_ShouldUpdatePope_WhenPopeExists()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var pope = new Pope { Name = "Old Name" };
        context.Popes.Add(pope);
        context.SaveChanges();

        var controller = new PopeController(context);
        var input = new popeReq_create
        {
            name = "Updated Name"
        };

        // Act
        var result = controller.UpdatePope(pope.Id, input);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result.Result);
        var updatedPope = Assert.IsType<Pope>(actionResult.Value);
        Assert.Equal("Updated Name", updatedPope.Name);
    }

    [Fact]
    public void DeletePope_ShouldReturnTrue_WhenPopeHasNoItems()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var pope = new Pope { Name = "Test Pope" };
        context.Popes.Add(pope);
        context.SaveChanges();

        var controller = new PopeController(context);

        // Act
        var result = controller.DeletePope(pope.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<bool>>(result);
        Assert.True(result.Value);
    }

    [Fact]
    public void DeletePope_ShouldReturnFalse_WhenPopeHasItems()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var pope = new Pope { Name = "Test Pope", Items = new List<Item> { new Item { Name = "Test Item", Description = "Lorem Ipsum Dolor Sit"} } };
        context.Popes.Add(pope);
        context.SaveChanges();

        var controller = new PopeController(context);

        // Act
        var result = controller.DeletePope(pope.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<bool>>(result);
        Assert.False(result.Value);
    }
}