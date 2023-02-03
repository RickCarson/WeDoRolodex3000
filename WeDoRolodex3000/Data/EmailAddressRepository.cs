namespace WeDoRolodex3000.Data;

public class EmailAddressRepository : BaseRepository<EmailAddress>
{
    public EmailAddressRepository(RolodexContext rolodexContext)
    {
        db = rolodexContext;
    }
}
