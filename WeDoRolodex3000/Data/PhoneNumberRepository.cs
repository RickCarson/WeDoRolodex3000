namespace WeDoRolodex3000.Data;

public class PhoneNumberRepository : BaseRepository<PhoneNumber>
{
    public PhoneNumberRepository(RolodexContext rolodexContext)
    {
        db = rolodexContext;
    }
}