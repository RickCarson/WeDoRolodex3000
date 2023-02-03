namespace WeDoRolodex3000.Data;

public class ContactRepository : BaseRepository<Contact>
{
    public ContactRepository(RolodexContext rolodexContext)
    {
        db = rolodexContext;
    }

    public override List<Contact> GetAll()
    {
        return db.Contacts.Include(a => a.PhoneNumbers)
            .Include(a => a.EmailAddresses)
            .Include(a => a.Notes)
            .ToList();
    }

    public override async Task<Contact> GetById(int id)
    {
        return GetAll()
            .FirstOrDefault(c => c.Id == id);
    }
}
