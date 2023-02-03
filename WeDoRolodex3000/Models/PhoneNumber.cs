namespace WeDoRolodex3000.Models;

public class PhoneNumber
{
    public int Id { get; set; }
    public int ContactId { get; set; }
    public string Number { get; set; }
    public string Type { get; set; }
}
