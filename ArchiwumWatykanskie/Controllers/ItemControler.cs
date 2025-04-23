using ArchiwumWatykanskie.Controllers.Requests;
using ArchiwumWatykanskie.Controllers.Responses;
using ArchiwumWatykanskie.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArchiwumWatykanskie.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemControler : ControllerBase
{
    [HttpPost("create")]
    public ActionResult<itemRes_get> CreateItem([FromBody] itemReq_create input)
    {
        using (var context = new AppDbContext())
        {
            var newItem = new Models.Item
            {
                Name = input.Name,
                Description = input.Description,
                PopeId = input.PopeId
            };
            context.Items.Add(newItem);
            context.SaveChanges();
            return CreatedAtAction(nameof(CreateItem), newItem);
        }
    }

    [HttpGet("get/{id}")]
    public ActionResult<itemRes_get> GetItem(int id)
    {
        using (var context = new AppDbContext())
        {
            var item = context.Items
                .Include(i => i.Pope)
                .FirstOrDefault(i => i.Id == id);
            if (item == null) return NotFound();
            itemRes_get itemResponse = new itemRes_get
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                PopeId = item.PopeId
            };
            
            return itemResponse;
        }
    }
    
    [HttpDelete("delete/{id}")]
    public ActionResult<bool> DeleteItem(int id)
    {
        using (var context = new AppDbContext())
        {
            var item = context.Items.Find(id);
            if (item == null) return NotFound();
            context.Items.Remove(item);
            context.SaveChanges();
            return true;
        }
    }
}