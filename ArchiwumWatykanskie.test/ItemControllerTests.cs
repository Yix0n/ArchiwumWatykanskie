using ArchiwumWatykanskie.Controllers;
using ArchiwumWatykanskie.Controllers.Requests;
using ArchiwumWatykanskie.Controllers.Responses;
using ArchiwumWatykanskie.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ArchiwumWatykanskie.test;

public class ItemControllerTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        var context = new AppDbContext(options);
        context.Database.OpenConnection();
        context.EnsureCreated();
        return context;
    }
    
    [Fact]
    public void CreateItem_ShouldReturnCreatedItem()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new ItemController(context);
        var input = new itemReq_create
        {
            Name = "Test Item",
            Description = "Test Description",
            PopeId = 1
        };

        // Act
        var result = controller.CreateItem(input);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdItem = Assert.IsType<itemRes_get>(actionResult.Value);
        Assert.Equal("Test Item", createdItem.Name);
    }

    [Fact]
    public void GetItem_ShouldReturnItem_WhenItemExists()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new ItemController(context);

        // Act
        var result = controller.GetItem(1);

        // Assert
        var actionResult = Assert.IsType<ActionResult<itemRes_get>>(result);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void DeleteItem_ShouldReturnTrue_WhenItemExists()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new ItemController(context);

        // Act
        var result = controller.DeleteItem(1);

        // Assert
        var actionResult = Assert.IsType<ActionResult<bool>>(result);
        Assert.True(result.Value);
    }
}