namespace WeDoRolodex3000.Models;

public class RolodexContext : DbContext
{
    public RolodexContext(DbContextOptions<RolodexContext> options)
        : base(options)
    {

    }

    public DbSet<Contact> Contacts { get; set; } = null!;
    public DbSet<PhoneNumber> PhoneNumbers { get; set; } = null!;
    public DbSet<EmailAddress> EmailAddresses { get; set; } = null!;
    public DbSet<Note> Notes { get; set; } = null!;
}
