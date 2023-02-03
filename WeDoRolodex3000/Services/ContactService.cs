
namespace WeDoRolodex3000.Services;

public class ContactService
{
    private readonly ILogger _logger;    
    private readonly ContactRepository _contactRepository;
    private readonly PhoneNumberRepository _phoneNumberRepository;
    protected readonly EmailAddressRepository _emailAddressRepository;
    protected readonly NotesRepository _notesRepository;

    public ContactService(ILogger<ContactService> logger
        , ContactRepository contactRepository
        , PhoneNumberRepository phoneNumberRepository
        , EmailAddressRepository emailAddressRepository
        , NotesRepository notesRepository)
    {
        _logger = logger;
        _contactRepository = contactRepository;
        _contactRepository = contactRepository;
        _phoneNumberRepository = phoneNumberRepository;
        _emailAddressRepository = emailAddressRepository;
        _notesRepository = notesRepository;
    }

    public async Task<ContactResponse> AddContact(Contact newContact)
    {
        _logger.LogInformation("Adding contact {@newContact}", newContact);
        try
        {
            await _contactRepository.Add(newContact);
            await _contactRepository.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return new ContactResponse(false, null!, ex.Message);
        }

        return new ContactResponse(true, newContact, "Contact Added");
    }

    public IEnumerable<Contact> GetPaginatedContacts(int size = 0, int page = 1)
    {
        _logger.LogInformation($"Getting Page {page} of Size {size}");
        try
        {
            if (size < 1)
                return _contactRepository.GetAll().OrderBy(c => c.FirstName);

            page = page < 1 ? 1 : page;
            var numberOfPages = _contactRepository.GetAll().Count() / size;
            page = page > numberOfPages ? numberOfPages : page;
            return _contactRepository.GetAll()
                .OrderBy(c => c.FirstName)
                .Skip(size * (page - 1))
                .Take(size);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return null;
        }
    }

    public async Task<ContactResponse> GetContactById(int contactId)
    {
        _logger.LogInformation($"Getting contact with Id {contactId}");
        try
        {
            var contact = await _contactRepository.GetById(contactId);

            if (contact != null)
                return new ContactResponse(true, contact, "Contact found");

            return new ContactResponse(false, contact, "Contact not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return new ContactResponse(false, null!, ex.Message);
        }
    }

    public async Task<ContactResponse> DeleteContactById(int contactId)
    {
        _logger.LogInformation($"Deleting contact with Id {contactId}");
        try
        {
            var result = await GetContactById(contactId);

            if (result.Success)
            {
                await _contactRepository.DeleteById(contactId);
                _contactRepository.SaveChanges();
                return new ContactResponse(true, result.Contact, "Contact deleted");
            }
            else
                return new ContactResponse(false, null!, "Contact not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return new ContactResponse(false, null!, ex.Message);
        }
    }

    public async Task<ContactResponse> UpdateContact(Contact contact)
    {
        _logger.LogInformation("Updating contact {@newContact}", contact);
        try
        {
            _contactRepository.Edit(contact);
            contact.PhoneNumbers.ToList().ForEach(p => _phoneNumberRepository.Edit(p));
            contact.EmailAddresses.ToList().ForEach(e => _emailAddressRepository.Edit(e));
            contact.Notes.ToList().ForEach(n => _notesRepository.Edit(n));
            await _contactRepository.SaveChanges();
            await _phoneNumberRepository.SaveChanges();
            await _emailAddressRepository.SaveChanges();
            await _notesRepository.SaveChanges();

            var result = await GetContactById(contact.Id);

            if (result.Success)
                return new ContactResponse(true, result.Contact, "Contact updated");

            return new ContactResponse(false, null!, "Contact not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return new ContactResponse(false, null!, ex.Message);
        }
    }

    public IEnumerable<Contact> SearchByName(string searchString)
    {
        _logger.LogInformation($"Searching for name containing {searchString}");
        try
        {
            return _contactRepository.GetAll().Where(c => c.FirstName.Contains(searchString) || c.LastName.Contains(searchString));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return null!;
        }
    }

    public void RefreshContactsData(Contact contact)
    {
        _logger.LogInformation($"Refreshing data {@contact}", contact);
        try
        {
            _contactRepository.RefreshData(contact);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
    }

}
