
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace WeDoRolodex3000.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ContactService _contactsService;

    public ContactsController(ILogger<ContactsController> logger, ContactService contactService)
    {
        _logger = logger;
        _contactsService = contactService;
    }


    // GET: api/<ContactsController>
    [HttpGet]
    public IActionResult Get()
    {
        var contacts = _contactsService.GetPaginatedContacts();

        if (contacts is null)
            return StatusCode(500);

        return Ok(contacts);
    }

    // GET api/<ContactsController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _contactsService.GetContactById(id);

        if (result?.Contact is null)
            return StatusCode(500);

        if (result.Success)
            return Ok(result.Contact);

        return NoContent();
    }

    // POST api/<ContactsController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Contact contact)
    {
        var result = await _contactsService.AddContact(contact);

        if (result.Success)
            return Ok(result.Contact);

        return StatusCode(500);
    }

    // PUT api/<ContactsController>/5
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Contact contact)
    {
        var result = await _contactsService.UpdateContact(contact);

        if (result?.Contact is null)
            return StatusCode(500);

        if (result.Success)
            return Ok(result.Contact);

        return NoContent();
    }

    // DELETE api/<ContactsController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _contactsService.DeleteContactById(id);

        if (result?.Contact is null)
            return StatusCode(500);

        if (result.Success)
            return Ok(result.Contact);

        return NoContent();
    }
}
