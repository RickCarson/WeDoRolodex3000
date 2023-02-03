namespace WeDoRolodex3000.Models;

public class Contact
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }

    public char Initial { get { return FirstName.Take(1).FirstOrDefault(); } }
    public string FullName { get { return $"{FirstName} {LastName}"; } }
    public string ShortName { get { return $"{Initial}. {LastName}"; } }
    public string FormalName { get { return $"{Title}. {Initial}. {LastName}"; } }

    public ICollection<PhoneNumber> PhoneNumbers { get; set; }
    public ICollection<EmailAddress> EmailAddresses { get; set; }
    public ICollection<Note> Notes { get; set; }
}