using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ContactsController : Controller
{
    private readonly ContactsAPIDbContext dbContext;

    public ContactsController(ContactsAPIDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllContacts()
    {
        return Ok(await dbContext.Contacts.ToListAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSingleContact(Guid id)
    {
        var contact = await dbContext.Contacts.FindAsync(id);

        if(contact == null) { return NotFound(); };

        return Ok(contact);
    }

    [HttpPost]
    public async Task<IActionResult> AddContact(AddContactRequest contactDto)
    {
      var contact = new Contact()
      {
          Id = Guid.NewGuid(),
          FullName = contactDto.FullName,
          Email = contactDto.Email,
          Phone = contactDto.Phone,
          Address = contactDto.Address
      };

       await dbContext.Contacts.AddAsync(contact);
       await dbContext.SaveChangesAsync();

        return Ok(contact);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateContact(Guid id, UpdateContactRequest contactUpdateDto) 
    {
        var contact = await dbContext.Contacts.FindAsync(id);
        if (contact == null) { return NotFound(); }

        contact.FullName = contactUpdateDto.FullName;
        contact.Email = contactUpdateDto.Email;
        contact.Phone = contactUpdateDto.Phone;
        contact.Address = contactUpdateDto.Address;

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteContact(Guid id)
    {
        var contact = await dbContext.Contacts.FindAsync(id);

        if (contact == null) { return NotFound(); };

        dbContext.Remove(contact);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}
