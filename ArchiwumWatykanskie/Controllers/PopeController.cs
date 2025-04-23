using ArchiwumWatykanskie.Controllers.Requests;
using ArchiwumWatykanskie.Controllers.Responses;
using ArchiwumWatykanskie.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArchiwumWatykanskie.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PopeController : ControllerBase
{
    [HttpPost("create")]
    public ActionResult<popeRes_create> CreatePope([FromBody] popeReq_create input)
    {
        using (var context = new AppDbContext())
        {
            var newPope = new Models.Pope
            {
                Name = input.name
            };
            context.Popes.Add(newPope);
            context.SaveChanges();
            return CreatedAtAction(nameof(CreatePope), newPope);
        }
    }
    
    [HttpGet("get/{id}")]
    public ActionResult<popeRes_get> GetPope(int id)
    {
        using (var context = new AppDbContext())
        {
            var pope = context.Popes
                .Include(p => p.Items)
                .FirstOrDefault(p => p.Id == id);
            if (pope == null) return NotFound();
            popeRes_get popeResponse = new popeRes_get
            {
                Id = pope.Id,
                Name = pope.Name,
                BirthDate = pope.BirthDate,
                DeathDate = pope.DeathDate,
                Items = pope.Items.Select(i => new itemRes_get
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                }).ToList()
            };
            
            return popeResponse;
        }
    }
    
    [HttpPut("update/{id}")]
    public ActionResult<popeRes_create> UpdatePope(int id, [FromBody] popeReq_create input)
    {
        using (var context = new AppDbContext())
        {
            var pope = context.Popes.Find(id);
            if (pope == null)
            {
                return NotFound();
            }
            pope.Name = input.name;
            context.SaveChanges();
            return Ok(pope);
        }
    }
    
    [HttpDelete("delete/{id}")]
    public ActionResult<bool> DeletePope(int id)
    {
        using (var context = new AppDbContext())
        {
            var pope = context.Popes
                .Include(p => p.Items)
                .FirstOrDefault(p => p.Id == id);
            if (pope == null)
            {
                return NotFound();
            }

            if (pope.Items != null)
            {
                return false;
            }
            context.Popes.Remove(pope);
            context.SaveChanges();
            return true;
        }
    }
}