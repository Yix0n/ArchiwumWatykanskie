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
    private readonly AppDbContext _context;

    public PopeController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpPost("create")]
    public ActionResult<popeRes_create> CreatePope([FromBody] popeReq_create input)
    {
        var newPope = new Models.Pope
        {
            Name = input.name
        };
        _context.Popes.Add(newPope);
        _context.SaveChanges();

        var response = new popeRes_create
        {
            Id = newPope.Id,
            Name = newPope.Name
        };

        return CreatedAtAction(nameof(CreatePope), response);
    }

    [HttpGet("get/{id}")]
    public ActionResult<popeRes_get> GetPope(int id)
    {
        var pope = _context.Popes
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

    [HttpPut("update/{id}")]
    public ActionResult<popeRes_create> UpdatePope(int id, [FromBody] popeReq_create input)
    {
        var pope = _context.Popes.Find(id);
        if (pope == null)
        {
            return NotFound();
        }
        pope.Name = input.name;
        _context.SaveChanges();
        return Ok(pope);
    }

    [HttpDelete("delete/{id}")]
    public ActionResult<bool> DeletePope(int id)
    {
        var pope = _context.Popes
            .Include(p => p.Items)
            .FirstOrDefault(p => p.Id == id);
        if (pope == null)
        {
            return NotFound();
        }

        if (pope.Items != null && pope.Items.Any())
        {
            return false;
        }
        _context.Popes.Remove(pope);
        _context.SaveChanges();
        return true;
    }
}