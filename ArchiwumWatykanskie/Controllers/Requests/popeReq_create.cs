namespace ArchiwumWatykanskie.Controllers.Requests;

public class popeReq_create
{
    public string name { get; set; }
    public DateTime birthDate { get; set; }
    public DateTime? deathDate { get; set; }
}