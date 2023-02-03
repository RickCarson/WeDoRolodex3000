
namespace WeDoRolodex3000.Tests;

[TestFixture]
public abstract class BaseTest
{
    protected readonly ILogger _logger;
    protected readonly RolodexContext _rolodexContext;
    protected readonly ContactRepository _contactRepository;
    protected readonly PhoneNumberRepository _phoneNumberRepository;
    protected readonly EmailAddressRepository _emailAddressRepository;
    protected readonly NotesRepository _notesRepository;
    protected readonly ContactService _contactService;

    public BaseTest()
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddDbContext<RolodexContext>(opt =>
                opt.UseInMemoryDatabase("Rolodex"))
            .AddTransient<ContactService>()
            .AddTransient<ContactRepository>()
            .AddTransient<PhoneNumberRepository>()
            .AddTransient<EmailAddressRepository>()
            .AddTransient<NotesRepository>()
            .BuildServiceProvider();

        _rolodexContext = serviceProvider.GetService<RolodexContext>();
        _contactRepository = serviceProvider.GetService<ContactRepository>();
        _phoneNumberRepository = serviceProvider.GetService<PhoneNumberRepository>();
        _emailAddressRepository = serviceProvider.GetService<EmailAddressRepository>();
        _notesRepository = serviceProvider.GetService<NotesRepository>();
        _contactService = serviceProvider.GetService<ContactService>();
    }

}
