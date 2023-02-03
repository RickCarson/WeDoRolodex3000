namespace WeDoRolodex3000.Models;

public class Note
{
    public int Id { get; set; }
    public int ContactId { get; set; }
    public string Notes { get; set; }
    public string Type { get; set; }
}
