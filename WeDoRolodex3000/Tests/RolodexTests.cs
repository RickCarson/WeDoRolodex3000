using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NuGet.Frameworks;
using NUnit.Framework;
using WeDoRolodex3000.Models;

namespace WeDoRolodex3000.Tests;

public class RolodexTests : BaseTest
{
    public RolodexTests() : base()
    {

    }

    [SetUp]
    public async Task Init()
    {
        var contacts = new List<Contact> { 
            new Contact 
            {
                FirstName = "Rick", 
                LastName = "Carson", 
                Title = "Mr",
                PhoneNumbers = new List<PhoneNumber>
                {
                    new PhoneNumber
                    {
                        Number = "11111 111111",
                        Type = "Mobile"
                    }
                },
                EmailAddresses = new List<EmailAddress>
                {
                    new EmailAddress {
                        Email = "Rick@Carson.com",
                        Type = "Personal"
                    }
                },
                Notes = new List<Note>
                {
                    new Note
                    {
                        Notes = "Great guy, good dev, well worth employing",
                        Type = "Public"                    
                    }
                }
            },
            new Contact 
            {
                FirstName = "Bill", 
                LastName = "Murray", 
                Title = "Mr",
                PhoneNumbers = new List<PhoneNumber>(),
                EmailAddresses = new List<EmailAddress>(),
                Notes = new List<Note>()
            }
        };

        contacts.ForEach(c => _contactRepository.Add(c));
        await _contactRepository.SaveChanges();
    }

    [TearDown]
    public async Task End()
    {
        _contactRepository.DeleteAll();
        await _contactRepository.SaveChanges();
    }

    [Test]
    public async Task When_Requesting_Paginated_Contacts_With_No_Parameters_All_Contacts_Returned()
    {
        var contacts = _contactService.GetPaginatedContacts();

        Assert.AreEqual(2, contacts.Count(), "Both contacts exists");

        var contact = contacts.Where(c => c.FirstName.Equals("Rick")).FirstOrDefault();

        Assert.IsNotNull(contact, "First contact exists");
        Assert.IsTrue(contact.FirstName.Equals("Rick"), "First name is correct");
        Assert.IsTrue(contact.LastName.Equals("Carson"), "Last name is correct");
        Assert.IsTrue(contact.Title.Equals("Mr"), "Title name is correct");
    }

    [Test]
    public async Task When_Requesting_Paginated_Contacts_With_Parameters_Correct_Page_Is_Returned()
    {
        var contacts = _contactService.GetPaginatedContacts(1, 1);

        Assert.AreEqual(1, contacts.Count(), "First page of one returned");

        var contact = contacts.Where(c => c.FirstName.Equals("Bill")).FirstOrDefault();

        Assert.IsNotNull(contact, "First page contact exists");

        contacts = _contactService.GetPaginatedContacts(1, 2);

        Assert.AreEqual(1, contacts.Count(), "Second page of one returned");

        contact = contacts.Where(c => c.FirstName.Equals("Rick")).FirstOrDefault();

        Assert.IsNotNull(contact, "Second page contact exists");
    }

    [Test]
    public async Task When_Requesting_Paginated_Contacts_If_Page_Is_Too_Small_Or_Great_Default_Will_Be_First_Or_Last()
    {
        var contacts = _contactService.GetPaginatedContacts(1, 0);

        Assert.AreEqual(1, contacts.Count(), "First page of one returned");

        var contact = contacts.Where(c => c.FirstName.Equals("Bill")).FirstOrDefault();

        Assert.IsNotNull(contact, "First page contact exists");

        contacts = _contactService.GetPaginatedContacts(1, 3);

        Assert.AreEqual(1, contacts.Count(), "Second page of one returned");

        contact = contacts.Where(c => c.FirstName.Equals("Rick")).FirstOrDefault();

        Assert.IsNotNull(contact, "Second page contact exists");
    }

    [Test]
    public async Task When_Data_Entered_Extra_Name_Fields_Should_Be_Calculated()
    {
        var contacts = _contactService.GetPaginatedContacts();

        var contact = contacts.FirstOrDefault();

        Assert.IsNotNull(contact, "First contact exists");
        Assert.IsTrue(contact.FullName.Equals("Bill Murray"), "FullName is correct");
        Assert.IsTrue(contact.ShortName.Equals("B. Murray"), "ShotName is correct");
        Assert.IsTrue(contact.FormalName.Equals("Mr. B. Murray"), "FormalName is correct");
    }

    [Test]
    public async Task When_Adding_New_Contact_It_Should_Be_Saved()
    {
        var result = await _contactService.AddContact(new Contact { FirstName = "John", LastName = "Wayne", Title = "Mr" });

        Assert.IsNotNull(result, "A result has been returned");
        Assert.IsTrue(result.Success, "Result is successful");
        Assert.IsTrue(result.Message.Equals("Contact Added"), "Success message returned");
        Assert.AreEqual(3, result.Contact.Id, "New record saved as Id 3");
    }

    [Test]
    public async Task When_Searching_By_Name_Correct_Records_Are_Returned()
    {
        var contacts = _contactService.SearchByName("Rick");

        Assert.AreEqual(1, contacts.Count(), "One contact returned");
        Assert.IsTrue(contacts.FirstOrDefault().FirstName.Equals("Rick"), "Correct contact returned");
    }

    [Test]
    public async Task When_Getting_Contact_By_Id_The_Correct_Contact_Should_Be_Returned()
    {
        var id = _contactService.SearchByName("Rick").FirstOrDefault().Id;

        var result = await _contactService.GetContactById(id);

        Assert.IsTrue(result.Success, "Result is successful");
        Assert.IsTrue(result.Message.Equals("Contact found"), "Found message returned");
        Assert.IsTrue(result.Contact.LastName.Equals("Carson"), "Correct contact returned");
    }

    [Test]
    public async Task When_Deleting_Existing_Contact_It_Is_Deleted()
    {
        var id = _contactService.SearchByName("Rick").FirstOrDefault().Id;

        var result = await _contactService.DeleteContactById(id);

        Assert.IsTrue(result.Success, "Result is successful");
        Assert.IsTrue(result.Message.Equals("Contact deleted"), "Found message returned");
        Assert.IsTrue(result.Contact.LastName.Equals("Carson"), "Correct contact deleted");

        result = await _contactService.GetContactById(id);

        Assert.IsFalse(result.Success, "Result is no longer a success");
        Assert.IsTrue(result.Message.Equals("Contact not found"), "Not found message returned");
        Assert.IsNull(result.Contact, "Contact no longer exists");
    }

    [Test]
    public async Task When_Editing_A_Record_The_Values_Are_Updated()
    {
        var contact = _contactService.SearchByName("Rick").FirstOrDefault();

        contact.FirstName = "Richard";

        var result = await _contactService.UpdateContact(contact);
        Assert.IsTrue(result.Message.Equals("Contact updated"), "Updated message returned");
        Assert.IsTrue(result.Contact.FirstName.Equals("Richard"), "First name has been updated");

        Assert.IsTrue(result.Success, "Result is successful");

        result = await _contactService.GetContactById(contact.Id);

        Assert.IsTrue(result.Success, "Result is successful");
        Assert.IsTrue(result.Message.Equals("Contact found"), "Found message returned");
        Assert.IsTrue(result.Contact.FirstName.Equals("Richard"), "First name is still updated after Get");
    }

    [Test]
    public async Task When_PhoneNumber_Email_And_Notes_All_Exist()
    {
        var contact = _contactService.SearchByName("Rick").FirstOrDefault();

        _contactService.UpdateContact(contact);

        var contactWithDetails = await _contactService.GetContactById(contact.Id);

        Assert.IsNotNull(contactWithDetails?.Contact?.PhoneNumbers, "Contact has phone numbers");
        Assert.IsNotNull(contactWithDetails?.Contact?.EmailAddresses, "Contact has email addresses");
        Assert.IsNotNull(contactWithDetails?.Contact?.Notes, "Contact has email Notes");
    }

}
