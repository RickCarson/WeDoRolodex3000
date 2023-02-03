namespace WeDoRolodex3000.Data;

public class NotesRepository : BaseRepository<Note>
{
    public NotesRepository(RolodexContext rolodexContext)
    {
        db = rolodexContext;
    }
}
