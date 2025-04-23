namespace ArchiwumWatykanskie.Models;

public class Pope
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Item> Items { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime DeathDate { get; set; }
}