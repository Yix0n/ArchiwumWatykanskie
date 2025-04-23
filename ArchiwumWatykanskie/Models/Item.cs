namespace ArchiwumWatykanskie.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public int PopeId { get; set; }
    public Pope Pope { get; set; }
}