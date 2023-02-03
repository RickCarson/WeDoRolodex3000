namespace WeDoRolodex3000.Models;

public class EmailAddress
{
    public int Id { get; set; } 
    public int ContactId { get; set; }
    public string Email { get; set; }
    public string Type { get; set; }
}
