using ArchiwumWatykanskie.Models;

namespace ArchiwumWatykanskie.Controllers.Responses;

public class popeRes_get
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public List<itemRes_get> Items { get; set; }
}